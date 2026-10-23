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

window.showWaitingPanel = function (message = "Proszę czekać...") {
    const loadText = document.getElementById("loadtextid");
    const loading = document.getElementById("loading");

    if (!loading || !loadText) {
        return;
    }

    loadText.textContent = typeof message === "string" && message.trim() !== ""
        ? message
        : "Proszę czekać...";
    loading.setAttribute("aria-hidden", "false");
    loading.style.display = "flex";
};

window.hideWaitingPanel = function () {
    const loading = document.getElementById("loading");

    if (loading) {
        loading.style.display = "none";
        loading.setAttribute("aria-hidden", "true");
    }
};

// Zachowanie zgodności z dotychczasowymi wywołaniami.
window.czekaj = window.showWaitingPanel;
window.hideLoader = window.hideWaitingPanel;

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

// zapis do pliku
async function zapiszElementJakoJpg(elementId, nazwaPliku = 'obraz.jpg') {

    const element = document.getElementById(elementId);

    if (!element) {
        alert(`Nie znaleziono elementu o id: ${elementId}`);
        return;
    }

    const canvas = await html2canvas(element, {
        scale: 2,
        backgroundColor: '#ffffff',
        useCORS: true
    });

    const link = document.createElement('a');

    link.download = nazwaPliku;
    link.href = canvas.toDataURL('image/jpeg', 0.95);

    link.click();
}

async function zapiszElementJakoJpg2(
    elementId,
    nazwaPliku = 'obraz.jpg',
    listaIdDoUkrycia = []
) {
    const element = document.getElementById(elementId);

    if (!element) {
        alert(`Nie znaleziono elementu o id: ${elementId}`);
        return;
    }

    // Znajdź elementy do ukrycia
    const elementyDoUkrycia = listaIdDoUkrycia
        .map(id => document.getElementById(id))
        .filter(el => el && element.contains(el));

    // Zapamiętaj dotychczasową widoczność
    const poprzedniaWidocznosc = elementyDoUkrycia.map(el => ({
        element: el,
        display: el.style.display
    }));

    try {
        // Ukryj wskazane elementy
        elementyDoUkrycia.forEach(el => {
            el.style.display = 'none';
        });

        const canvas = await html2canvas(element, {
            scale: 2,
            backgroundColor: '#ffffff',
            useCORS: true
        });

        const link = document.createElement('a');
        link.download = nazwaPliku;
        link.href = canvas.toDataURL('image/jpeg', 0.95);
        link.click();
    }
    catch (error) {
        console.error('Błąd podczas zapisywania JPG:', error);
        alert('Nie udało się zapisać obrazu.');
    }
    finally {
        // Przywróć widoczność elementów
        poprzedniaWidocznosc.forEach(item => {
            item.element.style.display = item.display;
        });
    }
}

