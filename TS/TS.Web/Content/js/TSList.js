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
            '<nav aria-label="Page navigation">',
                '<ul class="pagination">',
                    '<li class="first-page">',
                        '<a href="javascription:void(0)">',
                            '<span>首页</span>',
                        '</a>',
                    '</li>',
                    '<li class="pre-page">',
                        '<a href="javascription:void(0)">',
                            '<span>上一页</span>',
                        '</a>',
                    '</li>',
                    '<li class="next-page">',
                        '<a href="javascription:void(0)">',
                            '<span>下一页</span>',
                        '</a>',
                    '</li>',
                    '<li class="last-page">',
                        '<a href="javascription:void(0)">',
                            '<span>尾页</span>',
                        '</a>',
                    '</li>',
                    '<li class="turn">',
                        '<span class="input-group" style="margin-left:15px;border:0;padding:0">',
                            '<input class="turnTo form-control" type="text" class="form-control" style="width:50px">',
                        '</span>',
                    '</li>',
                    '<li class="turn">',
                        '<span style="background-color:#eee;color:#555">页</span>',
                    '</li>',
                    '<li class="confirm">',
                        '<a href="javascription:void(0)">',
                            '<span>确认</span>',
                        '</a>',
                    '</li>',
                '</ul>',
            '</nav>',
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
	        if (!that.options.totalCount || that.options.totalCount == 0)
	        {
	            $footer.hide();
	        }
	        $footer.append(that.options.footerHtml);

	        this.initBtn(that);
	    },
	    initBtn: function (that) {
	        var $footer = $("#" + that.options.footerContainerId + "");

	        that.options.pageCount = Math.ceil(that.options.totalCount / that.options.pageSize);
	        var pageShowCount = that.options.pageCount <= that.options.footerMaxPageShowSize ? that.options.pageCount : that.options.footerMaxPageShowSize;

	        docFragment = document.createDocumentFragment();
	        for (var i = 1; i <= pageShowCount; i++) {
	            var li = document.createElement('li');
	            var a = document.createElement('a');

	            a.innerHTML = i + '';
	            a.setAttribute("href", "javascription:void(0)")
	            if(i == 1){
	                li.setAttribute('class','page-text active');
	            } else {
	                li.setAttribute('class', 'page-text');
	            }

	            li.appendChild(a);
	            docFragment.appendChild(li);
	        }
	        var html = '<li><span class="totalPage" style="background-color:#eee;color:#555">共 ' + that.options.pageCount + ' 页</span></div></li>';
	        $footer.find('.pre-page').after(docFragment);
	        $footer.find('.last-page').after(html);

	        this.bindFooterEvent(that);
	    },
	    bindFooterEvent: function (that) {
	        var $footer = $("#" + that.options.footerContainerId + "");
	        var methods = this;
            //点击页面
	        $footer.on('click', '.page-text', function () {
	            $(this).addClass('active').siblings('.page-text').removeClass('active');
	            var index = window.parseInt($(this).find('a').text());
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
	            that.options.pageIndex = index > that.options.pagesCount ? that.options.pagesCount : index;
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
	                var turnto = $footer.find(".turnTo").val();
	                $footer.find(".turnTo").val('');

	                if (!(parseInt(turnto) == turnto)) {
	                    errPromot("请输入整数");
	                    return false;
	                }
	                if (isNaN(turnto)) {
	                    errPromot("请输入整数");
	                    return false;
	                }
	                if ((turnto > that.options.pageCount) || (turnto < 1)) {
	                    errPromot("输入的页码超出范围");
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
	        var pageCount = that.options.pageCount,
                index = that.options.pageIndex,
                startIndex = 0,
                endIndex = 0,
                rightCount = parseInt(that.options.btnShowCount / 2),
                footerMaxPageShowSize = that.options.footerMaxPageShowSize,
                $pageItems = $("#" + that.options.footerContainerId + "").find(".page-text");

	        if (pageCount > footerMaxPageShowSize) {
	            if (index + rightCount <= pagesCount) {
	                endIndex = index + rightCount;
	            } else {
	                endIndex = pagesCount;
	            }
	            endIndex = endIndex < showBtnsCount ? showBtnsCount : endIndex;//左侧开始的负数
	            startIndex = endIndex - showBtnsCount + 1;
	            $pageItems.each(function (index2) {
	                $(this).find('a').text(startIndex + index2);
	                if (startIndex + index2 === index) {
	                    $(this).addClass('active').siblings('.page-text').removeClass('active');
	                }
	            });
	        } else {
	            $pageItems.each(function (index2) {
	                if (index2 + 1 == index) {
	                    $(this).addClass('active').siblings('.page-text').removeClass('active');
	                }
	            });
	        }
	    },
	    updatePageCount: function (that) {
	        that.options.pageCount = Math.ceil(that.options.totalCount / that.options.pageSize);
	        $("#" + that.options.footerContainerId + "").find(".totalPage").html("共" + that.pageCount + "页");
	        this.handleIndex(that);
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
	                    if (method.hasValue(data.totalCount) && data.totalCount != 0) {
	                        that.options.totalCount = data.totalCount;
	                        method.updatePageCount(that);
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
	    refreshFooter: function () {
	        this.options.totalCount = null;
	        this.options.pageIndex = 1;

	        this.getData(this);
	    }

	}

	$.fn.TSList = function (options) {
		return new TSList($(this), options);
	};
})(jQuery, window, document);