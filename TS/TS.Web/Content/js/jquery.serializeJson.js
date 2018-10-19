$.fn.extend({
    serializeJson: function () {
        var $this = $(this);
        var json = {};

        $this.find("input,select").each(function (index, element) {
            var $node = $(element);
            var type = $node.attr("type");

            //输入框类型不为radio和checkbox或者已经被选中
            if ((type != "radio" && type != "checkbox") || $node.prop("checked") == true) {
                var name = $node.attr("name") ? $node.attr("id") : $node.attr("name");
                var value = $node.val();

                json[name] = value;
            }
        });

        //只能在form表单内使用
        //var list = $this.serializeArray();
        //var json = {};
        //for (var index in list) {
        //    json[list[index].name] = list[index].value;
        //}

        return json;
    }
});