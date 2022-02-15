function hidModal() {
    var x = document.getElementById("modal");
    if (x.style.display === "block") {
        x.style.display = "none";
        $(".modal-body>p").empty();;
    }
}