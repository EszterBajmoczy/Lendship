# Lendship

Wireframe: https://www.figma.com/file/Q4TTcQRk3jrnepK1b1p4PA/Wireframe?node-id=0%3A1

ER diagram: https://drive.google.com/file/d/1BAisfX0LS9fg8AQi5LGx2N1UEmplwU2M/view?usp=sharing

SwaggerHub: https://app.swaggerhub.com/apis/EszterBajmoczy/Lendship.Backend/1.0.0



# Haladási napló:
2022.04.28:
- GitHub Actions

2022.04.21:
- Frontend implementations
  - Login, Registration, Token handling
  - Advertisements pages
  - Messages page
  - Reservation for an advertisement
  - Profil page with evaluations
- Statikus képkiszolgálás
- Adatbázis feltöltő script

2022.03.23:
- Notification / History implementation/modification
  - kereshető a hirdetés címe alapján
- fix reservation state update
- about us information (appsettings.json-ben)
- modify closed group

2022.03.16:
- JustInMind UI tervek
- UI tervezés során felmerülő hiányosságok:
  - Szabadszavas keresés
  - History - foglalások / teljesült foglalások
  - Céges kontakt, about us page - legyen adatbázisba mentve vagy webbe beégetve?
  - Closed group hozzáadása - email alapján kellene lennie, ne listázza ki az embereket vagy igen? - akkor kellene azoknak a listázására is lekérdezés 

2021.11.15:
- Implement refresh token (issue 6)
- Cancel token with Redis db (issue 1)

Kérdések:
- Refresh token-t is deaktiválni kellene, de hogyan? Összes legenerált token számontartása? Token ellenőrzésnél akkor azt is nézni kellene, hogy tárolva van nálunk.

2021.11.01:
- Conversation controller és service megvalósítása
- Closed Group controller és service megvalósítása
- Ami még hátra van: 
  - Profile és Image implementációk

2021.10.24:
- Bejelentkezés megvalósítása, JWT token használatával (jelszó visszaállítás még hiányzik)
- Advertisement controller és service megvalósítása
- Reservation controller és service megvalósítása
- Evaluation controller-ek és service megvalósítása
- A project felépítése:
  - Models - EF modellek
  - DTOs - A kliensnek szükséges modellek
  - Converters - Modellek és Dto-k közötti átakalításért felelős osztályok
  - Controllers - A http kéréseket fogadó osztályok
  - Services - Az üzleti logikát megvalósító osztályok, Converterek használata
  - Interfaces - Converter-ek és Servic-ek interface-ei
- Swagger osztályok beépítése a projektbe

Kérdések:
- Képek:
  - File rendszerben tervezem őket tárolni
  - Külön http kérésekben utazzanak!
- Enum modelben és Dto-ban megegyezik
  - Maradjon ahogyan van, kettő enum és legyen hozzá converter 
- Amennyiben egy értékelés anonym, akkor a frontend anonimizálja vagy a backend?
  - Backend
- Értékelések számítása külön lekérdezésben legyen, vagy Dto-hoz hozzáfűzve?
  - Legyen egy napra kész, kiszámított érték (User táblába mehet) 

2021.10.10: Swagger-ben elkészítettem a backend api-ját. A projektbe holnap integrálom a generált file-okat.
