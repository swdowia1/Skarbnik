console.log("site.js został załadowany");
function textValue(id) {
    return $("#" + id).val();
}

function setValue(id, value) {
    $("#" + id).text(value);
}

load = function () {
    window.location.reload();
};

// =====================================
// Globalne okno potwierdzenia
// =====================================

let confirmCallback = null;
function showGlobalConfirmModal(callback, customMessage = null) {
    const modalElement = document.getElementById("globalConfirmModal");
    if (!modalElement) {
        console.error("Nie znaleziono modala globalConfirmModal.");
        return;
    } if (typeof callback !== "function") {
        console.error("Callback musi być funkcją.");
        return;
    }
    confirmCallback = callback;
    const messageElement = modalElement.querySelector(".confirm-modal-message");
    if (messageElement) {
        messageElement.textContent = customMessage ?? "Czy na pewno chcesz wykonać tę akcję?";
    }
    if (typeof bootstrap === "undefined" || !bootstrap.Modal) {
        console.error("Bootstrap Modal nie jest załadowany.");
        confirmCallback = null; return;
    }
    bootstrap.Modal.getOrCreateInstance(modalElement).show();
}

// Jeden handler — callback wykona się tylko raz.
$(document).on("click", "#confirmModalOkBtn", function () {
    const modalElement = document.getElementById("globalConfirmModal");

    const callback = confirmCallback;
    confirmCallback = null;

    if (modalElement && typeof bootstrap !== "undefined" && bootstrap.Modal) {
        bootstrap.Modal.getOrCreateInstance(modalElement).hide();
    }

    if (typeof callback === "function") {
        callback();
    }
});

// =====================================
// Loader
// =====================================

window.czekaj = function (title) {
    const loadText = document.getElementById("loadtextid");
    const loading = document.getElementById("loading");

    if (!loading || !loadText) {
        return;
    }

    if (title && title.trim() !== "") {
        loadText.textContent = title;
    }

    loading.style.display = "flex";
};

window.hideLoader = function () {
    const loading = document.getElementById("loading");

    if (loading) {
        loading.style.display = "none";
    }
};

// =====================================
// AJAX
// =====================================

postjson = function (n, t, i, r, u) {
    return $.ajax({
        url: n,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: i,
        complete: u,
        error: r,
        data: JSON.stringify(t)
    });
};

getjson = function (n, t, i, r) {
    return $.ajax({
        url: n,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: t,
        complete: r,
        error: i
    });
};

deletejson = function (n, t, i, r) {
    return $.ajax({
        url: n,
        type: "DELETE",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: t,
        complete: r,
        error: i
    });
};

gethtml = function (n, t, i, r) {
    return $.ajax({
        url: n,
        type: "GET",
        success: t,
        complete: r,
        error: i
    });
};

postdata = function (n, t, i, r, u) {
    return $.ajax({
        url: n,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: r,
        complete: i,
        error: u,
        data: JSON.stringify(t)
    });
};

