# Assignment 10 — Three Enemy Types

## Карта скриптов для видео

- Patroller → Patrol → Chase → WindUp → Recover → Patrol. Потеря игрока возвращает патруль.
- Shooter → Scan → Aim → Recover → Scan. Направление фиксируется при входе в Aim; EnemyProjectile проверяет стену и попадание.
- Charger → Guard → WindUp → Dash → Recover → Guard. Рывок ограничен дистанцией и границами площадки.
- Все три используют EnemyBrain для видимости, Raycast по маске стен, красного предупреждения и таймера. Урон передаётся в PlayerHealth, а тот обновляет HUD через MobileGameUI.

## Плейтест

Дайте телефон двум людям, которые раньше не видели игру. Запишите обоих в клипе 1–2 минуты, считайте попытки на втором уровне. Цель — победа за 2–4 попытки. Если возможно, проведите повторный тест после изменений.

| Игрок | Попыток до победы | Где и почему проигрывал |
| --- | --- | --- |
| 1 | | |
| 2 | | |

| Враг и параметр | Было | Стало | Причина и результат повторной проверки |
| --- | --- | --- | --- |
| | | | |
| | | | |

## Английский текст для записи

**0:00–0:20** — “This is Skybound. I’ll show the three enemies together on the second level and explain how each one decides when to attack.”

**0:20–1:20** — “The patroller walks between two points. It checks the player’s distance and angle, then casts a ray to check for walls. When it reaches the player, it flashes red before it strikes. The shooter scans the area, locks its aim, and fires after a warning. I can move away from the shot. The charger waits until I enter its lane, flashes red, and dashes. It then has to recover.” Show one dodge per enemy if possible.

**1:20–1:55** — “Each enemy has its own state machine. EnemyBrain shares the visibility checks and attack timing. The shooter creates an EnemyProjectile. All three send damage to PlayerHealth, which updates the hearts.” Show the script map above.

**1:55–2:30** — “Two new players tested the level on a phone. They took [actual attempts] to win. I changed [parameter] from [old] to [new] because [observed issue]. I also changed [parameter] from [old] to [new] because [observed issue]. After the changes, [actual result].”

## Canvas

Добавьте ссылку на репозиторий в комментарий к сдаче. В репозитории нужны коммиты за эту неделю и DECISIONS.md. Приложите APK или короткую запись с iPhone, клип плейтеста и отдельный MP4 на 2–3 минуты с объяснением на английском.
