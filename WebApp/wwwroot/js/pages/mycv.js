(() => {
    const root = document.querySelector('[data-privacy-toggle]');
    if (!root) return;

    const btn = root.querySelector('.privacy-switch');
    if (!btn) return;

    const ON_CLASS = 'is-on';

    // Starta PÅ just nu (tills ni kopplar mot SQL).
    // Sen kan ni sätta initial state från servern med t.ex. data-initial="on/off".
    setState(true);

    btn.addEventListener('click', () => {
        const next = !btn.classList.contains(ON_CLASS);
        setState(next);
    });

    function setState(isOn) {
        btn.classList.toggle(ON_CLASS, isOn);
        btn.setAttribute('aria-pressed', isOn ? 'true' : 'false');
    }
})();
