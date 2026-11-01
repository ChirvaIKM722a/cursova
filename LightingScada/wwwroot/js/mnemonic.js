(function () {
    const TOPIC_LIGHT  = 'lighting/1/sensors/room1/light';
    const TOPIC_MOTION = 'lighting/1/sensors/room1/motion';
    const LIGHT_THRESHOLD = 300;   // лк, як у LightingControlSystem
    const STALE_AFTER_MS = 6000;   // дані старші за 6 с вважаються застарілими
    const POLL_MS = 1000;

    const $ = id => document.getElementById(id);

    function setIndicator(id, reading) {
        const el = $(id);
        el.classList.remove('live', 'stale');
        if (!reading) return;
        const age = Date.now() - new Date(reading.receivedAtUtc).getTime();
        el.classList.add(age > STALE_AFTER_MS ? 'stale' : 'live');
    }

    function render(data) {
        const badge = $('mqtt-status');
        badge.textContent = data.connected ? "MQTT: підключено" : "MQTT: немає з'єднання";
        badge.className = 'badge ' + (data.connected ? 'on' : 'off');

        const byTopic = {};
        data.sensors.forEach(s => byTopic[s.topic] = s);
        const light = byTopic[TOPIC_LIGHT];
        const motion = byTopic[TOPIC_MOTION];

        if (light) {
            $('light-value').textContent = light.value.toFixed(1);
            $('light-unit').textContent = 'лк';
        }
        setIndicator('ind-light', light);

        if (motion) {
            $('motion-value').textContent = motion.value >= 1 ? 'Рух виявлено' : 'Руху немає';
            $('pir-dot').style.fill = motion.value >= 1 ? 'var(--ok)' : 'var(--off)';
        }
        setIndicator('ind-motion', motion);

        // Розрахунковий стан світильника (лише відображення)
        const lampOn = light && motion && motion.value >= 1 && light.value < LIGHT_THRESHOLD;
        const brightness = lampOn ? (1 - light.value / LIGHT_THRESHOLD) : 0;
        $('lamp').style.fill = lampOn ? 'var(--lamp)' : '';
        $('lamp-glow').setAttribute('opacity', (brightness * 0.6).toFixed(2));
        $('lamp-state').textContent = lampOn ? 'увімкнено, ' + Math.round(brightness * 100) + '%' : 'вимкнено';

        const times = data.sensors.map(s => new Date(s.receivedAtUtc).getTime());
        $('last-update').textContent = times.length
            ? 'Оновлено: ' + new Date(Math.max(...times)).toLocaleTimeString()
            : 'Оновлено: —';
    }

    async function poll() {
        try {
            const response = await fetch('/Home/GetSensors', { cache: 'no-store' });
            if (response.ok) render(await response.json());
        } catch (e) {
            const badge = $('mqtt-status');
            badge.textContent = 'Сервер недоступний';
            badge.className = 'badge off';
        }
    }

    poll();
    setInterval(poll, POLL_MS);
})();
