// mycv.js — placeholder toggle (kopplas mot SQL senare)
document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector(".cv-toggle");
    if (!toggle) return;

    // Default: ON (tills backend finns)
    setState(true);

    toggle.addEventListener("click", () => {
        const isOn = toggle.classList.contains("is-on");
        setState(!isOn);
    });

    function setState(on) {
        toggle.classList.toggle("is-on", on);
        toggle.setAttribute("aria-checked", on ? "true" : "false");

        // valfritt: kunna styla sidan beroende på läge senare
        document.documentElement.dataset.privateMode = on ? "on" : "off";
    }
});
