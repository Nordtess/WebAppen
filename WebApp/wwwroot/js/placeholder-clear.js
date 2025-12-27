// Clears placeholder text on focus and restores it on blur (if field is empty).
// Applies to any input/textarea with a `placeholder`.

(() => {
    // Initierar beteendet för alla input/textarea med placeholder inom angiven rot (standard: document).
    function init(root = document) {
        const fields = root.querySelectorAll('input[placeholder], textarea[placeholder]');

        fields.forEach((el) => {
            if (!(el instanceof HTMLInputElement || el instanceof HTMLTextAreaElement)) return;

            // Förhindra dubbelbindning genom att markera elementet som redan aktiverat beteendet
            if (el.dataset.phcBound === '1') return;
            el.dataset.phcBound = '1';

            const original = el.getAttribute('placeholder') ?? '';
            el.dataset.phcOriginal = original;

            // Rensa endast den visuella placeholder-texten på fokus; påverkar inte användarens inskrivna värde.
            el.addEventListener('focus', () => {
                el.setAttribute('placeholder', '');
            });

            // Återställ placeholder om fältet är tomt vid blur
            el.addEventListener('blur', () => {
                if ((el.value || '').trim().length === 0) {
                    el.setAttribute('placeholder', el.dataset.phcOriginal ?? original);
                }
            });
        });
    }

    document.addEventListener('DOMContentLoaded', () => init());
})();
