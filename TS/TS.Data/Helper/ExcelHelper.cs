using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TS.Data.Helper
{
    public partial class ExcelHelper
    {
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetName">页名</param>
        /// <param name="cellHead">属性的名称(key)和显示名称(value)</param>
        /// <param name="data">导出到excel数据源</param>
        /// <returns></returns>
        public XSSFWorkbook Export<T>(string sheetName, Dictionary<string, string> cellHead, IQueryable<T> data)
        {
            //HSSF使用于2007之前的xls版本，XSSF适用于2007及其之后的xlsx版本
            XSSFWorkbook xk = new XSSFWorkbook();
            ISheet sheet = xk.CreateSheet(sheetName);

            ICellStyle style = xk.CreateCellStyle();
            style.WrapText = true;
            IFont font = xk.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(font);

            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < cellHead.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(cellHead.ElementAt(i).Value);
                cell.SetCellType(CellType.String);
                cell.CellStyle = style;
            }

            int rowIndex = 0;
            foreach(var entity in data)
            {
                rowIndex++;
                row = sheet.CreateRow(rowIndex);    

                for (int j = 0; j < cellHead.Count; j++)
                {
                    var cellValue = string.Empty;
                    var property = entity.GetType().GetProperties().FirstOrDefault(e => e.Name == cellHead.ElementAt(j).Key);

                    if (property != null)
                    {
                        cellValue = property.GetValue(entity).ToString();
                        //对时间初始值赋值为空
                        if (cellValue.Trim() == "0001/1/1 0:00:00" || cellValue.Trim() == "0001/1/1 23:59:59")
                        {
                            cellValue = "";
                        }
                    }

                    row.CreateCell(j).SetCellValue(string.Empty);
                }
            }

            return xk;
        }

        /// <summary>
        /// 将导入Excel转换为对象列表，仅第一页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xk">excel对象</param>
        /// <param name="cellHead">属性的名称(key)和显示名称(value)</param>
        /// <param name="errmsg">错误信息</param>
        /// <returns></returns>
        public List<T> Import<T>(XSSFWorkbook xk, Dictionary<string, string> cellHead, out StringBuilder errmsg)
            where T : new ()
        {
            var list = new List<T>();
            errmsg = new StringBuilder();

             var sheet = xk.GetSheetAt(0);
            for (int i = 1; i < sheet.LastRowNum; i++)
            {
                // 判断当前行是否空行
                if (sheet.GetRow(i) == null)
                {
                    continue;
                }

                T entity = new T();
                string errStr = "";
                for (int j = 0; j < cellHead.Count; j++)
                {
                    var properotyInfo = entity.GetType().GetProperty(cellHead.ElementAt(j).Key);
                    if (properotyInfo != null)
                    {
                        try
                        {
                            // Excel单元格的值转换为对象属性的值，若类型不对，记录出错信息
                            properotyInfo.SetValue(entity, ExcelCellToProperty(properotyInfo.PropertyType, sheet.GetRow(i).GetCell(j)), null);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("excel导入时转换值失败", ex);

                            if (errStr.Length == 0)
                            {
                                errStr = "第" + i + "行数据转换异常：";
                            }
                            errStr += cellHead.ElementAt(j) + "列；";
                        }
                    }
                }
                // 若有错误信息，就添加到错误信息里
                if (errStr.Length > 0)
                {
                    errmsg.AppendLine(errStr);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 将excel表格的值转换对象的属性
        /// </summary>
        /// <param name="distanceType"></param>
        /// <param name="sourceCell"></param>
        /// <returns></returns>
        private static Object ExcelCellToProperty(Type distanceType, ICell sourceCell)
        {
            object rs = distanceType.IsValueType ? Activator.CreateInstance(distanceType) : null;

            // 判断传递的单元格是否为空
            if (sourceCell == null || string.IsNullOrEmpty(sourceCell.ToString()))
            {
                return rs;
            }

            // Excel文本和数字单元格转换，在Excel里文本和数字是不能进行转换，所以这里预先存值
            object sourceValue = null;
            switch (sourceCell.CellType)
            {
                case CellType.Blank:
                    break;

                case CellType.Boolean:
                    break;

                case CellType.Error:
                    break;

                case CellType.Formula:
                    break;

                case CellType.Numeric:
                    sourceValue = sourceCell.NumericCellValue;
                    break;

                case CellType.String:
                    sourceValue = sourceCell.StringCellValue;
                    break;

                case CellType.Unknown:
                    break;

                default:
                    break;
            }

            string valueDataType = distanceType.Name;

            // 在这里进行特定类型的处理
            switch (valueDataType.ToLower()) // 以防出错，全部小写
            {
                case "string":
                    rs = sourceValue.ToString();
                    break;
                case "int":
                case "int16":
                case "int32":
                    rs = (int)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    break;
                case "float":
                case "single":
                    rs = (float)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    break;
                case "datetime":
                    rs = sourceCell.DateCellValue;
                    break;
                case "guid":
                    rs = (Guid)Convert.ChangeType(sourceCell.NumericCellValue.ToString(), distanceType);
                    return rs;
            }
            return rs;
        }
    }
}
