# Dokumentace: Workout Tracker - UI/UX Redesign & Auth Systém

Tento dokument popisuje změny provedené v aplikaci Workout Tracker, zaměřené na modernizaci uživatelského rozhraní (UI/UX) a implementaci systému pro registraci a přihlašování uživatelů.

## 1. Architektonické změny a nové složky

Pro lepší organizaci kódu byly vytvořeny následující adresáře:
-   `Models/`: Obsahuje datové struktury (třída `User`).
-   `Services/`: Obsahuje logiku aplikace nezávislou na UI (třída `AuthService`).

## 2. Implementované komponenty

### A. Autentizační Systém
-   **`Models/User.cs`**: Definuje uživatele s vlastnostmi `Username`, `Password` (prostý text pro účely demo/školní práce) a `FullName`.
-   **`Services/AuthService.cs`**:
    *   Zajišťuje načítání a ukládání uživatelů do souboru `users.json`.
    *   Metoda `Register`: Ověřuje unikátnost jména a ukládá nového uživatele.
    *   Metoda `Login`: Ověřuje shodu jména a hesla, nastavuje `CurrentUser`.
-   **`users.json`**: Lokální databáze uživatelů ve formátu JSON.

### B. Uživatelské Rozhraní (UI)
-   **`App.axaml`**: 
    *   Kompletní předělávka stylů.
    *   Definovány globální barvy (Slate-900 pro pozadí, Indigo/Violet pro akcenty).
    *   Vytvořeny znovupoužitelné styly pro karty (`card`), moderní tlačítka (`primary`) a vstupy (`modern`).
-   **`LoginWindow.axaml / .cs`**:
    *   Nové startovací okno aplikace.
    *   Obsahuje přepínatelný pohled mezi Přihlášením a Registrací.
    *   Zajišťuje validaci vstupů a spouštění hlavní aplikace po úspěšném ověření.
-   **`MainWindow.axaml / .cs`**:
    *   **Sidebar**: Boční navigační panel s SVG ikonami a profilem uživatele.
    *   **Dashboard**: Hlavní plocha s kartami statistik (Tento týden, Celkový čas, Kalorie).
    *   **Dynamické prvky**: Texty v záhlaví a sidebaru se nyní dynamicky mění podle jména přihlášeného uživatele skrze metodu `LoadUserData()`.

## 3. Technické detaily změn v souborech

| Soubor | Typ změny | Popis |
| :--- | :--- | :--- |
| `Program.cs` | Konfigurace | Standardní inicializace Avalonia s fontem Inter. |
| `App.axaml.cs` | Logika startu | Změněno `desktop.MainWindow` na `LoginWindow`. |
| `MainWindow.axaml` | Layout | Přechod z jednoduchého Gridu na komplexní Grid s bočním panelem a UniformGridem pro statistiky. Opravena chyba s nekompatibilní vlastností `Spacing` u Gridu. |
| `WorkoutTracker.csproj` | Závislosti | Kontrola verzí Avalonia (11.x) pro zajištění kompatibility stylů. |

## 4. Jak systém funguje (Data Flow)

1.  **Start**: Aplikace spustí `LoginWindow`.
2.  **Registrace**: Uživatel zadá údaje -> `AuthService` zapíše do `users.json` -> Uživatel je vyzván k přihlášení.
3.  **Přihlášení**: `AuthService` porovná vstupy s daty v `users.json` -> Pokud souhlasí, uloží instanci uživatele do `AuthService.CurrentUser`.
4.  **Přechod**: `LoginWindow` vytvoří instanci `MainWindow`, zavolá `.Show()` a samo se zavře.
5.  **Personalizace**: `MainWindow` při inicializaci zavolá `LoadUserData()`, která přečte `FullName` z `AuthService.CurrentUser` a vypíše ho do UI.

## 5. Použité technologie
-   **Framework**: Avalonia UI (verze 11.3.9)
-   **Jazyk**: C# 12 / .NET 8
-   **Data**: System.Text.Json (perzistence do souboru)
-   **Design**: Moderní Flat Design / Dark Mode (inspirováno Tailwind CSS barvami)

## 6. Detailní rozbor kódu a funkcí (Code Walkthrough)

Tato sekce obsahuje podrobný přehled jednotlivých tříd a jejich funkcí pro lepší pochopení logiky aplikace.

### 6.1 `Program.cs`
- `Main(string[] args)`: Vstupní bod aplikace. Volá `BuildAvaloniaApp()` a spouští aplikaci s klasickým desktopovým životním cyklem (`StartWithClassicDesktopLifetime()`).
- `BuildAvaloniaApp()`: Inicializuje samotný framework Avalonia, nastavuje detekci platformy, výchozí písmo (Inter font) a interní mechanismus pro logování do konzole.

### 6.2 `App.axaml.cs`
- `Initialize()`: Načte veškeré vizuální definice XAML (styly, barvy) z `App.axaml`.
- `OnFrameworkInitializationCompleted()`: Volá se těsně před reálným spuštěním oken aplikace. Tato metoda se stará o to, že jako první okno pro uživatele je zvoleno `LoginWindow` místo původního hlavního okna.

### 6.3 `Models/User.cs`
Základní datová struktura pro jednoho uživatele.
- `Username`: Textový identifikátor uživatele (slouží jako "přihlašovací login").
- `Password`: Uživatelské heslo.
- `FullName`: Celé jméno uživatele, které slouží pro vizuální prezentaci uvnitř aplikace (např. "Jan Svoboda").

### 6.4 `Services/AuthService.cs`
Tato třída obsluhuje ověřování a perzistentní uložení dat pomocí JSON. Veškerá logika je statická.
- `CurrentUser`: Vlastnost, ve které je uložen aktuálně přihlášený uživatel po celou dobu běhu aplikace.
- `Register(string username, string password, string fullName)`: Přečte databázi, zkontroluje zda jméno neexistuje (ignoruje velikost písmen) a pokud je unikátní, založí nový objekt `User`, přidá ho do listu a zavolá ukládací funkci.
- `Login(string username, string password)`: Otevře databázi a hledá uživatele, kde heslo i přihlašovací jméno přesně sedí. Při úspěchu naplní vlastnost `CurrentUser` a vrátí `true`.
- `LoadUsers()`: Přečte text ze souboru `users.json` (pokud soubor existuje) a použije `JsonSerializer.Deserialize` pro převod JSON textu zpět na seznam objektů `List<User>`.
- `SaveUsers(List<User> users)`: Převede list instancí typu `User` do úhledného a strukturovaného formátu `json` (`WriteIndented = true`) a přepíše aktuální obsah souboru `users.json`.

### 6.5 `LoginWindow.axaml.cs`
Logika, která stojí za registračním a přihlašovacím rozhraním okna.
- `LoginWindow()`: Výchozí konstruktor, který připojí designové XAML prvky do C# kódu.
- `ToggleView(...)`: Přepíná vizuální zobrazení. Na starosti má skrytí panelu s přihlášením a zobrazení panelu s registrací (či naopak).
- `OnLoginClick(...)`: Seskupí data ze vstupních textových polí `TextBox` a ověří je funkcí `AuthService.Login()`. Jakmile se povede přihlášení, vygeneruje se instance pro `MainWindow` (otevře se dashboard) a Login okno zavolá `this.Close()`, čímž se samo bezpečně ukončí.
- `OnRegisterClick(...)`: Vyhodnotí a zkontroluje vstupní pole, ověří, zda uživatel nevyplnil prázdné vstupy a skrze `AuthService.Register()` zajistí vložení nového uživatele. O chybách informuje barevnými červenými hláškami (`LoginError`, `RegError`).

### 6.6 `MainWindow.axaml.cs`
Hlavní dashboard s navigací, jakmile je uživatel úspěšně přihlášen.
- `MainWindow()`: Připojí XAML styly a následně se rovnou postará o inicializaci uvítacích textů spuštěním `LoadUserData()`.
- `LoadUserData()`: Pokud existuje přihlášený uživatel v `AuthService.CurrentUser`, funkce sáhne do vlastnosti `FullName`. Následně vezme pouze první křestní jméno použitím `.Split(' ')[0]` (rozdělení dle mezery) a vytvoří uvítací větu do komponenty `WelcomeTextBlock` v hlavičce obrazovky. Společně s tím zapíše kompletní název uživatele i do profilového popisku `SidebarUserName` umístěného na dně bočního panelu.

### 6.7 `Models/Workout.cs`
Reprezentuje jeden tréninkový záznam.
- `Id`: Unikátní identifikátor (Guid).
- `UserUsername`: Vazba na uživatele, který trénink vytvořil.
- `ExerciseType`: Typ aktivity (např. "Běh").
- `Date`: Datum konání tréninku.
- `DurationMinutes`: Délka trvání v minutách.
- `Intensity`: Intenzita zátěže.

### 6.8 `Services/WorkoutService.cs`
Služba pro správu tréninků ukládaných do `workouts.json`.
- `SaveWorkout(Workout workout)`: Přidá nový trénink do souboru.
- `GetUserWorkouts(string username)`: Vrátí seznam všech tréninků konkrétního uživatele seřazený od nejnovějšího.

### 6.9 Aktualizace `MainWindow.axaml.cs` pro tréninky
- `UserWorkouts`: Kolekce typu `ObservableCollection`, která automaticky aktualizuje UI při přidání prvku.
- `OnSaveWorkoutClick(...)`: Obsluha tlačítka "Uložit trénink". Načte data z formuláře, vytvoří objekt `Workout`, uloží jej skrze `WorkoutService` a okamžitě aktualizuje seznam na obrazovce.

### 6.10 `AddWorkoutWindow.axaml / .cs`
Nové vyskakovací okno pro přidání tréninku.
- Nahrazuje původní "Rychlý záznam" na hlavní ploše.
- **Funkce**:
    - `OnSaveClick`: Vytvoří a uloží trénink, nastaví příznak `IsSaved = true` a zavře okno.
    - `OnCancelClick`: Zavře okno bez uložení.
- V `MainWindow` se otevírá asynchronně pomocí `ShowDialog(this)`, což zajistí, že se seznam tréninků aktualizuje až po zavření okna.

### 6.11 `WorkoutHistoryWindow.axaml / .cs`
Okno pro zobrazení kompletní historie tréninků uživatele.
- **Vizuální styl**: Tréninky jsou zobrazeny v "bublinách" (kartách) vedle sebe pomocí `WrapPanel`.
- **Logika**:
    - Přijímá seznam všech tréninků v konstruktoru.
    - Zobrazuje celkový počet tréninků v barevném odznaku (badge) v záhlaví.
- Na hlavním dashboardu (`MainWindow`) jsou nyní zobrazeny pouze **3 nejaktuálnější tréninky**, zbytek je dostupný právě v tomto okně po kliknutí na "Zobrazit vše".

### 6.12 Navigace a Sidebar
- **Odstranění "Nový trénink"**: Pro zjednodušení rozhraní byla tato položka ze sidebaru odstraněna. Přidávání probíhá výhradně přes tlačítko v hlavičce.
- **Položka "Historie"**: Nyní funguje jako rychlá navigace. Kliknutím na tuto položku v sidebaru se otevře stejné okno historie jako při kliknutí na "Zobrazit vše" na dashboardu.

---
*Dokumentace vytvořena dne 6. března 2026.*