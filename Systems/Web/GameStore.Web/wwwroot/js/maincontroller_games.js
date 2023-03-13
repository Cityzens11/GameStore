function selectGenres(pageNumber) {
    // Retrieve the selected genres and build a Filter object
    var selectedGenres = [];
    var checkboxes = document.getElementsByName('genre');
    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            selectedGenres.push(encodeURIComponent('Genres') + '=' + encodeURIComponent(checkboxes[i].value));
        }
    }

    // Concatenate the list of parameters into a query string
    var queryString = selectedGenres.join('&');

    var filter = { Name: '', Genres: queryString };

    // Redirect to the Games action with the filter as a parameter
    window.location.href = '/Games/GetGames?page=' + pageNumber + '&Name=' + encodeURIComponent(filter.Name) + '&' + filter.Genres;
}

function searchName(pageNumber) {
    // Retrieve the searched name
    var search = document.getElementById("search_input").value;

    // Redirect to the Games action with the filter as a parameter
    if (search.length > 3) {
        window.location.href = '/Games/GetGames?page=' + pageNumber + '&Name=' + encodeURIComponent(search);
    }
    else {
        document.getElementById("search_input").value = "";
        document.getElementById("search_input").setAttribute("placeholder", "Enter at least 3 chars");
    }
}
