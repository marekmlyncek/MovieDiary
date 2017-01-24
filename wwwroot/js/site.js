// Write your Javascript code.
function searchMovies() {
    var text = $("#search-movie").val();
    if (text == "") {
        $("#dropdown-holder").removeClass("open");
    }
    var url = "https://api.themoviedb.org/3/search/movie?api_key=f8f661c81ae1fb3899160a8b16245b63&query=" + text;
    $.ajax({url: url, success: function(respone) {
        if (respone.results.length > 0) {
            for (var i = 0; i < 5; i++) {
                if (respone.results[i] != null) {
                    if (respone.results[i].poster_path != null) {
                        var text =
                            "<li>" +
                                "<a href='/Movies/Detail/"+
                                respone.results[i].id+
                                "'>"+
                                "<img class=' img-search-poster' src='https://image.tmdb.org/t/p/w45_and_h67_bestv2/" +
                                respone.results[i].poster_path +
                                "' alt='" +
                                respone.results[i].title +
                                " poster." +
                                "'/>" +
                                "<span class='search-movies-text'>"+
                                respone.results[i].title +
                                "</span>"+
                                "</a>"+
                            "</li>";
                    } else {
                        var text =
                            "<ahref='/Movies/Detail/"+
                            respone.results[i].id+
                            "'>"+
                            "<li><img class='img-search-poster' src='images/poster_search_place_holder.png'" +
                            "alt='Poster unabelibe.'/>" +
                            respone.results[i].title +
                            "</a>"+
                        "</li>";
                    }
                    if (i == 0) {
                        $("#search-result-dropdown").html(text);
                    } else {
                        $("#search-result-dropdown").append("<li class='divider'></li>");
                        $("#search-result-dropdown").append(text);
                    }
                    $("#dropdown-holder").addClass("open");
                }
            }
        } else {
            $("#dropdown-holder").removeClass("open");
        }
    }});
}