(function () {
    // Kräver jQuery + jQuery Validate + Unobtrusive (via _ValidationScriptsPartial)
    function configureValidationBlurOnly() {
        if (!window.jQuery || !jQuery.validator) return;

        // Blur-only validation (inte varje tangenttryck)
        jQuery.validator.setDefaults({
            onkeyup: false,
            onclick: false,
            onfocusout: function (element) {
                // Validera när man lämnar fältet
                this.element(element);
            }
        });
    }

    function wireTextareaCounters() {
        const counters = document.querySelectorAll(".editcv-counter[data-for]");
        counters.forEach(counter => {
            const id = counter.getAttribute("data-for");
            const field = document.getElementById(id);
            if (!field) return;

            const max = field.getAttribute("maxlength") || "—";
            const update = () => {
                const len = (field.value || "").length;
                counter.textContent = `${len}/${max}`;
            };

            field.addEventListener("input", update);
            update();
        });
    }

    function scrollToFirstError(form) {
        const firstInvalid = form.querySelector(".input-validation-error");
        if (!firstInvalid) return;

        firstInvalid.scrollIntoView({ behavior: "smooth", block: "center" });
        firstInvalid.focus({ preventScroll: true });
    }

    function wireSubmitGuard() {
        const form = document.querySelector(".editcv-form");
        if (!form) return;

        form.addEventListener("submit", function (e) {
            // Om unobtrusive finns, kör den valideringen:
            if (window.jQuery && jQuery.validator) {
                const $form = jQuery(form);
                if ($form.valid && !$form.valid()) {
                    e.preventDefault();
                    scrollToFirstError(form);
                    return;
                }
            }

            // Fallback: HTML5 (om validate-lib saknas)
            if (!form.checkValidity()) {
                e.preventDefault();
                scrollToFirstError(form);
            }
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        configureValidationBlurOnly();
        wireTextareaCounters();
        wireSubmitGuard();
    });
})();
