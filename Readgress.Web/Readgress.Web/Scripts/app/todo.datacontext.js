﻿window.todoApp = window.todoApp || {};

window.todoApp.datacontext = (function (ko) {

    var datacontext = {
        getTodoLists: getTodoLists,
        createTodoItem: createTodoItem,
        createTodoList: createTodoList,
        saveNewTodoItem: saveNewTodoItem,
        saveNewTodoList: saveNewTodoList,
        saveChangedTodoItem: saveChangedTodoItem,
        saveChangedTodoList: saveChangedTodoList,
        deleteTodoItem: deleteTodoItem,
        deleteTodoList: deleteTodoList
    };

    return datacontext;

    function getTodoLists(todoListsObservable, errorObservable) {
        return ajaxRequest("get", todoListUrl())
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedTodoLists = $.map(data, function (list) { return new createTodoList(list); });
            todoListsObservable(mappedTodoLists);
        }

        function getFailed() {
            errorObservable("Error retrieving todo lists.");
        }
    }
    function createTodoItem(data) {
        return new datacontext.TodoItem(data); // TodoItem is injected by model.js
    }
    function createTodoList(data) {
        return new datacontext.TodoList(data); // TodoList is injected by model.js
    }
    function saveNewTodoItem(todoItem) {
        clearErrorMessage(todoItem);
        return ajaxRequest("post", todoItemUrl(), todoItem)
            .done(function (result) {
                todoItem.TodoItemId = result.TodoItemId;
            })
            .fail(function () {
                todoItem.ErrorMessage("Error adding a new todo item.");
            });
    }
    function saveNewTodoList(todoList) {
        clearErrorMessage(todoList);
        return ajaxRequest("post", todoListUrl(), todoList)
            .done(function (result) {
                todoList.TodoListId = result.TodoListId;
                todoList.UserId = result.UserId;
            })
            .fail(function () {
                todoList.ErrorMessage("Error adding a new todo list.");
            });
    }
    function deleteTodoItem(todoItem) {
        return ajaxRequest("delete", todoItemUrl(todoItem.TodoItemId))
            .fail(function () {
                todoItem.ErrorMessage("Error removing todo item.");
            });
    }
    function deleteTodoList(todoList) {
        return ajaxRequest("delete", todoListUrl(todoList.TodoListId))
            .fail(function () {
                todoList.ErrorMessage("Error removing todo list.");
            });
    }
    function saveChangedTodoItem(todoItem) {
        clearErrorMessage(todoItem);
        return ajaxRequest("put", todoItemUrl(todoItem.TodoItemId), todoItem)
            .fail(function () {
                todoItem.ErrorMessage("Error updating todo item.");
            });
    }
    function saveChangedTodoList(todoList) {
        clearErrorMessage(todoList);
        return ajaxRequest("put", todoListUrl(todoList.TodoListId), todoList)
            .fail(function () {
                todoList.ErrorMessage("Error updating the todo list title. Please make sure it is non-empty.");
            });
    }

    // Private
    function clearErrorMessage(entity) { entity.ErrorMessage(null); }
    function ajaxRequest(type, url, data) { // Ajax helper
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
    function todoListUrl(id) { return "/api/todolist/" + (id || ""); }
    function todoItemUrl(id) { return "/api/todo/" + (id || ""); }

})(ko);