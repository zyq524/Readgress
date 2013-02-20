window.readgressApp.readgressViewModel = (function (ko, datacontext) {
    var books = ko.observableArray(),
        error = ko.observable(),
        searchTitle = ko.observable(),
        searchBooks = function () {
            datacontext.getBookList(searchTitle(), books, error);
        };

    return {
        error: error,
        searchTitle: searchTitle,
        searchBooks: searchBooks,
        books: books
    };
})(ko, readgressApp.datacontext);

ko.applyBindings(window.readgressApp.readgressViewModel);