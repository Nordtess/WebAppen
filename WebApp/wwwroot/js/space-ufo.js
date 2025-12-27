(function () {
    // Hoppa över på små skärmar för att undvika prestandapåverkan på mobila enheter
    const main = document.querySelector('.app-main');
    if (!main) return;

    if (window.matchMedia && window.matchMedia('(max-width: 800px)').matches) return;

    // Säkerställ att ett lager för UFO:n finns ovanför innehållet
    let layer = document.getElementById('ufoLayer');
    if (!layer) {
        layer = document.createElement('div');
        layer.id = 'ufoLayer';
        layer.setAttribute('aria-hidden', 'true');
        main.insertBefore(layer, main.firstChild);
    }

    // Förhindra flera schemaläggningar om skriptet körs flera gånger
    if (window.__spaceUfoTimer) {
        clearTimeout(window.__spaceUfoTimer);
        window.__spaceUfoTimer = null;
    }

    const UFO_INTERVAL_MS = 20_000;
    const INITIAL_DELAY_MS = 7_000;
    const TRAVEL_MS = 6_000;

    function flyUfo() {
        const width = main.clientWidth;
        const height = main.clientHeight;
        const scrollTop = main.scrollTop;

        // Placera UFO:t i övre delen av den synliga viewporten
        const y = scrollTop + height * 0.22;

        // Start utanför vänsterkanten, slut utanför högerkanten
        const startX = -260;
        const endX = width + 260;

        const ufo = document.createElement('div');
        ufo.className = 'space-ufo';
        ufo.setAttribute('aria-hidden', 'true');

        // Fallback-storlek; primärt styrt via CSS
        ufo.style.width = '180px';
        ufo.style.height = '120px';

        layer.appendChild(ufo);

        const anim = ufo.animate(
            [
                { transform: `translate3d(${startX}px, ${y}px, 0)` },
                { transform: `translate3d(${endX}px, ${y}px, 0)` }
            ],
            {
                duration: TRAVEL_MS,
                easing: 'linear',
                fill: 'forwards'
            }
        );

        // Ta bort elementet när animationen är klar; extra fallback-timer för säker rensning
        anim.addEventListener('finish', () => ufo.remove());
        window.setTimeout(() => ufo.remove(), TRAVEL_MS + 500);
    }

    // Schemalägg nästa flygning (rekursiv timeout)
    function scheduleNext(delayMs) {
        window.__spaceUfoTimer = window.setTimeout(() => {
            flyUfo();
            scheduleNext(UFO_INTERVAL_MS);
        }, delayMs);
    }

    // Starta efter initial fördröjning och fortsätt med jämnt intervall
    scheduleNext(INITIAL_DELAY_MS);
})();
