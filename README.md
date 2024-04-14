# Drone Hunters
Dit is een schoolproject in samenwerking met [NewSecure](https://www.newsecure.nl). Het doel van dit project is het maken van een algoritme dat een drone autonoom achter een andere drone kan vliegen gebaseerd op de binnenkomende data van een spotter. Het programma wordt gesimuleerd en gevisualiseerd in Unity.

## Instalatie:
Om de simulatie te runnen:
1. Open de folder `Builds/Demo_01`
2. Voor het programma `drone_hunters.exe` uit

Om aan het project te werken:
1. Installeer [Unity versie 2022.3.10f1](https://unity.com/releases/editor/whats-new/2022.3.10)  
2. Clone de Repository
```bash
git clone https://github.com/saooms/DroneHunters.git
```
3. Open het project in Unity

## Applicate structuur:
In Unity werk je in de folder `Assets`. Hier vindt je de gebruikelijke folders zoals `Materials`, `Prefabs` en `Scripts`. De folder `Tests` is waar de unit tests staan, hier maken we gebruik van het [Unity Test Framework](https://docs.unity3d.com/Packages/com.unity.test-framework@1.4/manual/index.html).

De `Scipts` folder is verder verdeeld in `API`, `Logic` en `Visual`, bekijk en lees de klassendiagram voor meer informatie voor deze verdeling.
