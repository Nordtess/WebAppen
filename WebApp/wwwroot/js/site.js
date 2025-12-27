document.addEventListener("click", (e) => {
    const btn = document.querySelector(".nl-burger");
    const panel = document.querySelector(".nl-menu-panel");
    if (!btn || !panel) return;

    const clickedBtn = btn.contains(e.target);
    const clickedPanel = panel.contains(e.target);

    // Sätter aria + klasser för open/closed i en enhetlig funktion
    const setMenuState = (button, panelEl, isOpen) => {
        panelEl.classList.toggle("open", isOpen);
        button.classList.toggle("is-open", isOpen);
        button.setAttribute("aria-expanded", isOpen ? "true" : "false");
        panelEl.setAttribute("aria-hidden", isOpen ? "false" : "true");
    };

    const closeMenu = () => setMenuState(btn, panel, false);

    if (clickedBtn) {
        e.preventDefault();
        e.stopPropagation();

        const isOpen = !panel.classList.contains("open");
        setMenuState(btn, panel, isOpen);
        return;
    }

    if (!clickedPanel && panel.classList.contains("open")) {
        closeMenu();
    }
});

document.addEventListener("keydown", (e) => {
    if (e.key !== "Escape") return;

    const btn = document.querySelector(".nl-burger");
    const panel = document.querySelector(".nl-menu-panel");
    if (!btn || !panel) return;

    panel.classList.remove("open");
    btn.classList.remove("is-open");
    btn.setAttribute("aria-expanded", "false");
    panel.setAttribute("aria-hidden", "true");
    btn.focus();
});

document.addEventListener("DOMContentLoaded", () => {
    // Stäng nl-menu-panel när en menypost klickas
    document.querySelectorAll(".nl-menu-item").forEach((item) => {
        item.addEventListener("click", () => {
            const btn = document.querySelector(".nl-burger");
            const panel = document.querySelector(".nl-menu-panel");
            if (!btn || !panel) return;

            panel.classList.remove("open");
            btn.classList.remove("is-open");
            btn.setAttribute("aria-expanded", "false");
            panel.setAttribute("aria-hidden", "true");
        });
    });

    // Mail-indikator: visa antal och växla ikon efter unread
    const mail = document.querySelector(".nl-mail");
    if (mail) {
        const unread = parseInt(mail.dataset.unread || "0", 10);
        const closedSrc = mail.dataset.closedSrc;
        const openSrc = mail.dataset.openSrc;

        const img = mail.querySelector(".nl-mail-img");
        const countEl = mail.querySelector(".nl-unread-count");

        if (countEl) countEl.textContent = String(Math.max(0, unread));

        if (img && closedSrc && openSrc) {
            img.src = unread > 0 ? openSrc : closedSrc;
        }

        if (unread <= 0) {
            const txt = mail.querySelector(".nl-mail-text");
            if (txt) txt.innerHTML = "Inga nya meddelanden";
        }
    }

    /* --- User menu (toggle + click outside to close + Escape handling) --- */
    document.addEventListener("click", (e) => {
        const toggle = document.querySelector(".user-menu-toggle");
        const dropdown = document.querySelector(".user-dropdown");

        if (!toggle || !dropdown) return;

        const clickedToggle = toggle.contains(e.target);
        const clickedDropdown = dropdown.contains(e.target);

        if (clickedToggle) {
            e.preventDefault();
            e.stopPropagation();

            const isOpen = dropdown.getAttribute("aria-hidden") === "false";
            dropdown.setAttribute("aria-hidden", isOpen ? "true" : "false");
            toggle.setAttribute("aria-expanded", isOpen ? "false" : "true");
            return;
        }

        if (!clickedDropdown && dropdown.getAttribute("aria-hidden") === "false") {
            dropdown.setAttribute("aria-hidden", "true");
            toggle.setAttribute("aria-expanded", "false");
        }
    });

    document.addEventListener("keydown", (e) => {
        if (e.key !== "Escape") return;

        const toggle = document.querySelector(".user-menu-toggle");
        const dropdown = document.querySelector(".user-dropdown");

        if (!toggle || !dropdown) return;

        if (dropdown.getAttribute("aria-hidden") === "false") {
            dropdown.setAttribute("aria-hidden", "true");
            toggle.setAttribute("aria-expanded", "false");
            toggle.focus();
        }
    });

    // Stäng user-dropdown när en menypost i dropdown klickas
    document.querySelectorAll(".dropdown-item").forEach((item) => {
        item.addEventListener("click", () => {
            const toggle = document.querySelector(".user-menu-toggle");
            const dropdown = document.querySelector(".user-dropdown");

            if (!toggle || !dropdown) return;

            dropdown.setAttribute("aria-hidden", "true");
            toggle.setAttribute("aria-expanded", "false");
        });
    });
});

/* --- Resize: stänger öppna user-dropdown vid fönsterändring (debounced) --- */
let resizeTimer;
window.addEventListener("resize", () => {
    clearTimeout(resizeTimer);
    resizeTimer = setTimeout(() => {
        const toggle = document.querySelector(".user-menu-toggle");
        const dropdown = document.querySelector(".user-dropdown");

        if (!toggle || !dropdown) return;

        if (dropdown.getAttribute("aria-hidden") === "false") {
            dropdown.setAttribute("aria-hidden", "true");
            toggle.setAttribute("aria-expanded", "false");
        }
    }, 250);
});

/* --- Sidebar / hamburger (app-sidebar) --- */
(function () {
    const hamburger = document.querySelector(".hamburger-btn");
    const sidebar = document.querySelector(".app-sidebar");
    const overlay = document.querySelector(".sidebar-overlay");
    const sidebarLinks = document.querySelectorAll(".sidebar-link");

    if (!hamburger || !sidebar || !overlay) return;

    // Öppna/stäng sidebar: håller aria + overlay state i synk
    function openSidebar() {
        sidebar.classList.add("open");
        overlay.classList.add("active");
        hamburger.setAttribute("aria-expanded", "true");
    }

    function closeSidebar() {
        sidebar.classList.remove("open");
        overlay.classList.remove("active");
        hamburger.setAttribute("aria-expanded", "false");
    }

    function toggleSidebar() {
        const isOpen = sidebar.classList.contains("open");
        if (isOpen) {
            closeSidebar();
        } else {
            openSidebar();
        }
    }

    hamburger.addEventListener("click", (e) => {
        e.preventDefault();
        e.stopPropagation();
        toggleSidebar();
    });

    overlay.addEventListener("click", () => {
        closeSidebar();
    });

    document.addEventListener("keydown", (e) => {
        if (e.key !== "Escape") return;
        if (sidebar.classList.contains("open")) {
            closeSidebar();
        }
    });

    sidebarLinks.forEach((link) => {
        link.addEventListener("click", () => {
            if (sidebar.classList.contains("open")) {
                closeSidebar();
            }
        });
    });

    // Stänger sidebar automatiskt vid resize till bredare vy (debounced)
    let sidebarResizeTimer;
    window.addEventListener("resize", () => {
        clearTimeout(sidebarResizeTimer);
        sidebarResizeTimer = setTimeout(() => {
            if (window.innerWidth > 800 && sidebar.classList.contains("open")) {
                closeSidebar();
            }
        }, 250);
    });
})();
