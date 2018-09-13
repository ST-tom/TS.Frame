$.fn.extend({
    serializeJson: function () {
        var $this = $(this);
        var list = $this.serializeArray();
        var json = {};
        for (var index in list) {
            json[list[index].name] = list[index].value;
        }
        return json;
    }
})