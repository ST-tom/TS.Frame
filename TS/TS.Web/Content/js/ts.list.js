; (function ($, window, document, undefined){
    var defaults = {
        getPageListDataUrl: "",
        pageIndex: 1,
        pageSize: 10,
        searchBtnId: "",
        searchEleClass: "",
        searchEleChangeEvent: true,
        extendSerialize: null,
        afterSuccess: null,
        beforeSend: null,
        error: null,
        complete: null,
        errorShow:null,
    }

    //页脚属性
    var footerConfig = {
        footerContainerId: "",
        footerMaxPageShowSize: 5,
        footerFirstBtnShow: true,
        footerTurnBtnShow: false,
        totalCount: null,
        pageCount: null,
        footerHtml: [
            '<span class="page-footer">',
                '<span class="first-page">首页</span>',
                '<span class="pre-page">上一页</span>',
                '<span class="next-page">下一页</span>',
                '<span class="last-page">尾页</span>',
                '<span class="turnTo">',
                    '第<input type="text" />页',
                '</span>',
                '<span class="confirm">确认</span>',              
            '</span>',
        ].join(''),
    }

    //私有方法
	var method = {
	    defaultSuccess: function (data,that) {
	        if (!data.result) {
	            this.defaultErrorShow(that, "获取数据失败");
	        } else {
	            that.element.empty();
	            that.element.append(data.htmlStr);
	        }
	    },
	    defaultErrorShow:function(that,errmsg){
	        if ($.isFunction(that.options.errorShow)) {
	            that.options.errorShow(errmsg);
	        }else if(errmsg){
	            alert(errmsg);
	        }else{
	            alert("操作失败");
	        }
	    },
	    initFooter: function (that) {
	        if (!that.options.footerContainerId) {
	            return false;
	        }
	        var $footer = $("#" + that.options.footerContainerId + "");
	        $footer.append(that.options.footerHtml);

	        this.initBtn(that);
	        this.bindFooterEvent(that);
	    },
	    initBtn: function (that) {
	        var $footer = $("#" + that.options.footerContainerId + "");
	        $footer.find("a").remove();
	        $footer.find(".totalPage").remove();

	        that.options.pageCount = Math.ceil(that.options.totalCount / that.options.pageSize);
	        var pageShowCount = that.options.pageCount <= that.options.footerMaxPageShowSize ? that.options.pageCount : that.options.footerMaxPageShowSize;

	        docFragment = document.createDocumentFragment();
	        for (var i = 1; i <= pageShowCount; i++) {
	            var aNode = document.createElement('a');

	            aNode.innerHTML = i + '';
	            aNode.setAttribute("href", "javascription:void(0)")
	            if(i == 1){
	                aNode.setAttribute('class', 'active');
	            } 

	            docFragment.appendChild(aNode);
	        }
	        var html = '<span class="totalPage">共 ' + that.options.pageCount + ' 页</span>';
	        $footer.find('.pre-page').after(docFragment);
	        $footer.find('.last-page').after(html);

	        method.switchIndex(that);
	    },
	    bindFooterEvent: function (that) {
	        var $footer = $("#" + that.options.footerContainerId + "");
	        var methods = this;
            //点击页面
	        $footer.on('click', 'a', function () {
	            $(this).addClass('active').siblings('a').removeClass('active');
	            var index = window.parseInt($(this).text());
	            that.options.pageIndex = index;
	            methods.switchIndex(that);
	        });
            //点击前一页，后一页
	        $footer.on('click', '.pre-page', function () {
	            var index = that.options.pageIndex;
	            index--;
	            that.options.pageIndex = index < 1 ? 1 : index;
	            methods.switchIndex(that);
	        });
	        $footer.on('click', '.next-page', function () {
	            var index = that.options.pageIndex;
	            index++;
	            that.options.pageIndex = index > that.options.pageCount ? that.options.pageCount : index;
	            methods.switchIndex(that);
	        });

	        //首页和末页按钮
	        if (that.options.footerFirstBtnShow) {
	            $footer.on('click', '.first-page', function () {
	                that.options.pageIndex = 1;
	                methods.switchIndex(that);
	            });
	            $footer.on('click', '.last-page', function () {
	                that.options.pageIndex = that.options.pageCount;
	                methods.switchIndex(that);
	            });
	        } else {
	            $footer.find('.first-page, .last-page').hide();
	        }

	        //调整按钮
	        if (that.options.footerTurnBtnShow) {
	            $footer.on('click', '.confirm', function () {
	                var turnto = $footer.find(".turnTo input").val();
	                $footer.find(".turnTo input").val('');

	                if (!(parseInt(turnto) == turnto)) {
	                    methods.defaultErrorShow(that,"请输入整数");
	                    return false;
	                }
	                if (isNaN(turnto)) {
	                    methods.defaultErrorShow(that, "请输入整数");
	                    return false;
	                }
	                if ((turnto > that.options.pageCount) || (turnto < 1)) {
	                    methods.defaultErrorShow(that, "输入的页码超出范围");
	                    return false;
	                }

	                that.options.pageIndex = turnto;
	                methods.switchIndex(that);
	            });
	        } else {
	            $footer.find(".confirm, .turn").hide();
	        }
	    },
	    switchIndex: function (that) {
	        that.getData(that)
	        this.handleIndex(that);
	    },
	    handleIndex: function (that) {
	        $footer = $("#" + that.options.footerContainerId + "");
	        if (that.options.totalCount == 0) {
	            $footer.hide();
	        } else {
	            $footer.show();
	        }

	        var pageCount = that.options.pageCount,
                index = that.options.pageIndex,
                startIndex = 0,
                endIndex = 0,
                rightCount = parseInt(that.options.footerMaxPageShowSize / 2),
                footerMaxPageShowSize = that.options.footerMaxPageShowSize,
                $pageItems = $("#" + that.options.footerContainerId + "").find("a");

	        if (pageCount > footerMaxPageShowSize) {
	            if (index + rightCount <= pageCount) {
	                endIndex = index + rightCount;
	            } else {
	                endIndex = pageCount;
	            }
	            endIndex = endIndex < footerMaxPageShowSize ? footerMaxPageShowSize : endIndex;//左侧开始的负数
	            startIndex = endIndex - footerMaxPageShowSize + 1;
	            $pageItems.each(function (index2) {
	                $(this).text(startIndex + index2);
	                if (startIndex + index2 === index) {
	                    $(this).addClass('active').siblings('a').removeClass('active');
	                }
	            });
	        } else {
	            $pageItems.each(function (index2) {
	                if (index2 + 1 == index) {
	                    $(this).addClass('active').siblings('a').removeClass('active');
	                }
	            });
	        }
	    },
	    hasValue: function (data) {
	        if (data != null && data != "" && data != undefined) {
	            return true;
	        } else {
	            return false;
	        }
	    }
	}

    //构造函数
	function TSList(element, opts) {
	    this.element = element;
	    this.options = $.extend({}, defaults, footerConfig, opts);
	    this.init();
	}

    //原型(公开方法)
	TSList.prototype = {
	    init: function () {
	        var that = this;
	        that.bindEvent();
	        that.getData(this, true);
	        method.initFooter(this);
	    },
        //绑定事件（搜索）
	    bindEvent: function () {
	        var that = this;
	        if (method.hasValue(that.options.searchBtnId)) {
	            $("#" + that.options.searchBtnId + "").bind("click", function () {
	                that.getData(that,true);
	            });
	        }
	        if (that.options.searchEleChangeEvent) {
	            if (method.hasValue(that.options.searchEleClass)) {
	                $("." + that.options.searchEleClass + "").bind("change", function () {
	                    that.getData(that, true);
	                });
	            }
	        }
	    },
	    getData: function (that, needRefresh) {
	        if (needRefresh) {
	            that.options.pageIndex = 1;
	        }

	        $.ajax({
	            url: that.options.getPageListDataUrl,
	            type: "get",
	            async: false,
	            data: that.getSearchData(),
	            beforeSend: function () {
	                that.beforeSend();
	            },
	            success: function (data) {
	                that.success(data);
	                if (needRefresh) {
	                    if (data.result) {
	                        that.options.totalCount = data.totalCount;
	                        method.initBtn(that);
	                    }
	                }
	            },
	            error: function () {
	                that.error();
	            },
	            complete: function () {
	                that.complete();
	            },
	        });
	    },
	    getSearchData: function () {
	        var searchData = {};

	        if (method.hasValue(this.options.searchEleClass)) {
	            $("." + this.options.searchEleClass).each(function () {
	                var type = $(this).attr("type");
	                if (type == "radio" || type == "checkbox") {
	                    if ($(this).prop("checked")) {
	                        var key = $(this).attr("name");
	                        var value = $(this).val();
	                        if (!data[key]) {
	                            data[key] = [];
	                        }
	                        data[key].push(value);
	                    }
	                } else {
	                    var key = $(this).attr("name");
	                    var value = $(this).val();
	                    searchData[key] = value;
	                }
	            });
	        }

	        searchData.pageIndex = this.options.pageIndex;
	        searchData.pageSize = this.options.pageSize;

	        return this.extendSerialize(searchData);
	    },
        //额外自定义搜索数据
	    extendSerialize: function (searchData) {
	        if ($.isFunction(this.options.extendSerialize)) {
	            this.options.extendSerialize(searchData);
	        } else if(method.hasValue(this.options.extendSerialize)) {
	            $.extend(searchData, this.options.extendSerialize);
	        }
	        return searchData;
	    },
	    beforeSend: function () {
	        if ($.isFunction(this.options.beforeSend)) {
	            this.options.beforeSend();
	        }
	    },
	    success: function (data) {
	        if ($.isFunction(this.options.success)) {
	            this.options.success(data);
	        } else {
	            method.defaultSuccess(data,this);
	        }
	    },
	    error: function () {
	        if ($.isFunction(this.options.error)) {
	            this.options.error();
	        } else {
	            method.defaultErrorShow(this,"获取数据失败");
	        }
	        return false;
	    },
	    complete: function () {
	        if ($.isFunction(this.options.complete)) {
	            this.options.complete();
	        }
	    },
	    refreshFooter: function (needRefresh) {
	        this.options.totalCount = null;
	        this.options.pageIndex = 1;

	        this.getData(this, needRefresh);
	    }

	}

	$.fn.TSList = function (options) {
		return new TSList($(this), options);
	};
})(jQuery, window, document);