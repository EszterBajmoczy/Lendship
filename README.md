# Lendship

Wireframe: https://www.figma.com/file/Q4TTcQRk3jrnepK1b1p4PA/Wireframe?node-id=0%3A1

ER diagram: https://drive.google.com/file/d/1BAisfX0LS9fg8AQi5LGx2N1UEmplwU2M/view?usp=sharing

SwaggerHub: https://app.swaggerhub.com/apis/EszterBajmoczy/Lendship.Backend/1.0.0



# Haladási napló:
2021.10.24:
- Bejelentkezés megvalósítása, JWT token használatával (jelszó visszaállítás még hiányzik)
- Advertisement controller és service megvalósítása
- A project felépítése:
  - Models - EF modellek
  - DTOs - A kliensnek szükséges modellek
  - Converters - Modellek és Dto-k közötti átakalításért felelős osztályok
  - Controllers - A http kéréseket fogadó osztályok
  - Services - Az üzleti logikát megvalósító osztályok, Converterek használata
- Swagger osztályok beépítése a projektbe

Kérdések:
- Képek:
  - File rendszerben tervezem őket tárolni
  - Külön http kérésekben utazzanak, vagy fűzzem hozzá a megfelelő Dto osztályhoz?

2021.10.10: Swagger-ben elkészítettem a backend api-ját. A projektbe holnap integrálom a generált file-okat.
