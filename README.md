# Sajam knjiga – OISISI projekat 2025/2026

Informacioni sistem uprave sajma knjiga – desktop aplikacija za referenta sajma (C#, WPF, MVC).

## Struktura rešenja

- `Core` – modeli, DAO sloj (čuvanje u datoteke), servisi
- `ConsoleClient` – konzolna aplikacija (domaći zadatak 1)
- `WpfClient` – WPF aplikacija
- `Podaci` – JSON datoteke sa stanjem sistema (pravi se automatski pri prvom pokretanju, u korenu rešenja)

## Raspodela posla

### Student 1 – (Ognjen Raonić, RA204-2022)

Domaći zadatak 1:

- model podataka: `Posetilac` (+ `StatusPosetioca`), `Kupovina`, `Izdavac`
- serijalizacija i deserijalizacija u JSON datoteke (`Core/Utils/Serijalizacija.cs`, `PosetilacDao`, `KupovinaDao`, `IzdavacDao`)
- servisi: `PosetilacService`, `KupovinaService`, `IzdavacService`
- 8.1–8.4: dodavanje, izmena, brisanje i prikaz posetilaca, kupovina i izdavača u konzoli (`ConsoleClient`)

### Student 2 – (ime i prezime, broj indeksa)

- model podataka: `Autor`, `Knjiga`, `Adresa`
- (dopuniti)
