window.readgressApp = window.readgressApp || {};

window.readgressApp.datacontext = (function (ko) {
    var datacontext = {
        getBookList: getBookList,
        getBookByOLId: getBookByOLId,
        getProgressList: getProgressList,
        getBookmarkList: getBookmarkList,
        createBookList: createBookList,
        createBook: createBook,
        createProgressList: createProgressList,
        createBookmarkList: createBookmarkList,
        saveNewProgressItem: saveNewProgressItem,
        saveNewBookmarkItem: saveNewBookmarkItem,
        deleteBookmarkItem: deleteBookmarkItem
    };

    return datacontext;

    function getBookList(bookTitle, bookListObservable, errorObservable) {
        return ajaxRequest("get", bookByTitleUrl(bookTitle))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedBookList = createBookList(data);
            bookListObservable = mappedBookList.books();
        }

        function getFailed() {
            errorObservable("Error retrieving books.");
        }
    }

    function getBookByOLId() {

    }

    function getProgressList(progressListObservable, errorObservable) {
        return ajaxRequest("get", progressUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedProgressList = $.map(data, function (list) { return new createProgressList(list); });
            progressListObservable(mappedProgressList);
        }

        function getFailed() {
            errorObservable("Error retrieving progress list.");
        }
    }

    function getBookmarkList(bookmarkListObservable, errorObservable) {
        return ajaxRequest("get", bookmarkUrl(bookmarkListObservable.progressId, true))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedBookmarkList = $.map(data, function (list)
            {
                return new createBookmarkList(list);
            });
            bookmarkListObservable(mappedBookmarkList);
        }

        function getFailed() {
            errorObservable("Error retrieving bookmark list.");
        }
    }

    function createBookList(data) {
        return new datacontext.bookList(data);
    }

    function createBook(data) {
        return new datacontext.book(data);
    }

    function createProgressList(data) {
        return new datacontext.progressList(data);
    }

    function createBookmarkList(data) {
        return new datacontext.bookmarkList(data);
    }

    function saveNewProgressItem(progressItem) {
        clearErrorMessage(progressItem);
        return ajaxRequest("post", progressUrl(), progressItem)
            .done(function (result) {
                progressItem.Id = result.Id;
            })
        .fail(function () {
            progressItem.ErrorMessage("Error adding a new progress.");
        });
    }

    function saveNewBookmarkItem(bookmarkItem) {
        clearErrorMessage(bookmarkItem);
        return ajaxRequest("post", bookmarkUrl("", false), bookmarkItem)
            .done(function (result) {
                bookmarkItem.Id = result.Id;
            })
        .fail(function () {
            bookmarkItem.ErrorMessage("Error adding a new bookmark.");
        });
    }

    function deleteBookmarkItem(bookmarkItem) {
        return ajaxRequest("delete", bookmarkUrl(bookmarkItem.Id, false))
            .fail(function () {
                bookmarkItem.ErrorMessage("Error removing bookmark.");
        });
    }

    //Private
    function clearErrorMessage(entity) { entity.ErrorMessage(null); }

    function ajaxRequest(type, url, data) {
        var options = {
            dataType: "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: ko.toJSON(data)
        };
        return $.ajax(url, options);
    }

    // routes
    function progressUrl(id) { return "/api/progress/" + (id || ""); }
    function bookmarkUrl(id, isProgressId) {
        if (isProgressId) {
            return "/api/bookmark/?ProgressId=" + id;
        }
        else {
            return "/api/bookmark/" + (id || "");
        }
    }
    function bookByTitleUrl(title) { return "/api/book/?Title=" + title; }
    function bookUrl(oLId) { return "/api/book/?OLId=" + oLId; }
})(ko);