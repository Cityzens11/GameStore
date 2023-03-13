// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Get the "Sign in" link element
const signInLink = document.querySelector('.nav-link1');

// Get the dialog box element
const dialogBox = document.querySelector('.dialog-box');

const dialogOverlay = document.createElement('div');
dialogOverlay.classList.add('dialog-overlay');

// Add an event listener to the link
signInLink.addEventListener('click', (event) => {
    event.preventDefault(); // prevent the default link behavior of redirecting to a new page
    document.body.appendChild(dialogOverlay);
    // Add the "show" class to the dialog box to display it
    dialogBox.classList.add('show');
    dialogOverlay.classList.add('show');
});


// Add an event listener to the close button
dialogBox.querySelector('.closeButton').addEventListener('click', () => {
    // Remove the "show" class from the dialog box to hide it
    dialogOverlay.remove();
    dialogBox.classList.remove('show');
});