(function (ko, datacontext){
    datacontext.bookList = bookList;
    //datacontext.progressList = progressList;
    //datacontext.bookmarkList = bookmarkList;
    datacontext.book= book;
    //datacontext.progressItem = progressItem;
    //datacontext.bookmarkItem = bookmarkItem;

    function book(data) {
        var self = this;
        data = data || {};

        self.Title = ko.observable(data.Title);
        self.SubTitle = ko.observable(data.SubTitle);
        self.Subjects = ko.observableArray();
        self.Authors = ko.observableArray();
        self.Url = ko.observable(data.Url);
        self.Identifiers = ko.observableArray();
        self.Publishers = ko.observableArray();
        self.Publish_Places = ko.observableArray();
        self.Cover = ko.observable({
            Small: data.Cover.Small || "",
            Medium: data.Cover.Medium || "",
            Large: data.Cover.Large || ""
        });
        self.Number_Of_Pages = ko.observable(data.Number_Of_Pages);

        self.ErrorMessage = ko.observable();
    }

    function bookList(data) {
        var self = this;
        data = data || {};

        self.books = ko.observableArray(importBookItems(data));

        self.ErrorMessage = ko.observable();
    }

    function importBookItems(bookItems) {
        return $.map(bookItems || [],
            function (bookItemData) {
                return datacontext.createBook(bookItemData);
            });
    }
})(ko, readgressApp.datacontext)