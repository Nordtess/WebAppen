(function () {
    const main = document.querySelector('.app-main');
    if (!main) return;

    // Hoppa över funktionen på mindre skärmar av prestandaskäl
    if (window.matchMedia && window.matchMedia('(max-width: 800px)').matches) return;

    // Säkerställ att ett lager finns för meteorer (ligger ovanför bakgrunden)
    let layer = document.getElementById('meteorLayer');
    if (!layer) {
        layer = document.createElement('div');
        layer.id = 'meteorLayer';
        layer.setAttribute('aria-hidden', 'true');
        main.insertBefore(layer, main.firstChild);
    }

    // Undvik flera schemalagda instanser om skriptet laddas om
    if (window.__spaceMeteorTimer) {
        clearTimeout(window.__spaceMeteorTimer);
        window.__spaceMeteorTimer = null;
    }

    const METEOR_INTERVAL_MS = 20_000;
    const INITIAL_DELAY_MS = 2_000;
    const TRAVEL_MS = 6_000;

    function shootMeteor() {
        const width = main.clientWidth;
        const height = main.clientHeight;
        const scrollTop = main.scrollTop;

        // Startposition i det synliga viewport-området (höger-övre hörn)
        const startX = Math.max(0, width - 10);
        const startY = scrollTop + 10;

        // Slutposition (tillräckligt vänster för att helt lämna viewport)
        const endX = -260;
        const endY = scrollTop + height * 0.88;

        const meteor = document.createElement('div');
        meteor.className = 'space-meteor';
        meteor.setAttribute('aria-hidden', 'true');

        // Storlek kontrolleras via CSS; satt som fallback
        meteor.style.width = '420px';
        meteor.style.height = '225px';

        layer.appendChild(meteor);

        const anim = meteor.animate(
            [
                { transform: `translate3d(${startX}px, ${startY}px, 0)` },
                { transform: `translate3d(${endX}px, ${endY}px, 0)` }
            ],
            {
                duration: TRAVEL_MS,
                easing: 'linear',
                fill: 'forwards'
            }
        );

        // Ta bort element när animationen är klar (och en säkerhets-timer)
        anim.addEventListener('finish', () => meteor.remove());
        window.setTimeout(() => meteor.remove(), TRAVEL_MS + 500);
    }

    // Schemalägg nästa meteor med given fördröjning (rekursiv timeout)
    function scheduleNext(delayMs) {
        window.__spaceMeteorTimer = window.setTimeout(() => {
            shootMeteor();
            scheduleNext(METEOR_INTERVAL_MS);
        }, delayMs);
    }

    // Starta efter initial fördröjning, därefter med fast intervall
    scheduleNext(INITIAL_DELAY_MS);
})();
