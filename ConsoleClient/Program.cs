using System.Text;
using ConsoleClient.Controllers;
using Core.DAO;
using Core.Interfaces;
using Core.Services;

Console.OutputEncoding = Encoding.UTF8;

// Ovde se povezuju zavisnosti: DAO -> Service -> Controller
// DAO objekti pri kreiranju ucitavaju podatke iz datoteka (folder Podaci)
IPosetilacDao posetilacDao = new PosetilacDao();
IKupovinaDao kupovinaDao = new KupovinaDao();
IIzdavacDao izdavacDao = new IzdavacDao();

UserService userService = new UserService(new UserDao());
PosetilacService posetilacService = new PosetilacService(posetilacDao, kupovinaDao);
KupovinaService kupovinaService = new KupovinaService(kupovinaDao, posetilacDao);
IzdavacService izdavacService = new IzdavacService(izdavacDao);

ConsoleController controller = new ConsoleController(userService, posetilacService, kupovinaService, izdavacService);
controller.Start();

// cuvanje stanja pri zatvaranju (podaci se cuvaju i posle svake promene)
posetilacDao.Sacuvaj();
kupovinaDao.Sacuvaj();
izdavacDao.Sacuvaj();
