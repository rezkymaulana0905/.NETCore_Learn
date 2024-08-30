// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const trigger = document.querySelector(".toggle-btn");

const collapseButton = document.querySelector(".btn-collapse");

trigger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("shrink");
    document.querySelector("#content").classList.toggle("expand");

    $('.collapse').collapse('hide');

})
