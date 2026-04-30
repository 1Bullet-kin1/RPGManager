using RPGManager.Models;

namespace RPGManager.Data
{
    public static class DataSeeder
    {
        public static void Seed(DndDBContext db)
        {
            // Если данные уже есть — не заполняем повторно
            if (db.Worlds.Any()) return;

            // Миры
            var toril = new World { Name = "Торил", Description = "Мир Забытых Королевств. Планета со множеством континентов, богатой историей и пантеоном богов." };
            db.Worlds.Add(toril);
            db.SaveChanges();

            // Континенты
            var faerun = new Continent { Name = "Фаэрун", Description = "Западный континент Торила. Колыбель большинства событий Забытых Королевств.", WorldId = toril.Id };
            var karatur = new Continent { Name = "Кара-Тур", Description = "Восточный континент с культурами, напоминающими Восточную Азию.", WorldId = toril.Id };
            db.Continents.AddRange(faerun, karatur);
            db.SaveChanges();

            // Регионы
            var swordCoast = new Region { Name = "Побережье Мечей", Description = "Полоса прибрежных городов-государств вдоль западного побережья Фаэруна.", ContinentId = faerun.Id };
            var north = new Region { Name = "Север", Description = "Холодные дикие земли к северу от Побережья Мечей.", ContinentId = faerun.Id };
            var anauroch = new Region { Name = "Анурох", Description = "Огромная пустыня в центре Фаэруна. Некогда великая империя Нетерил.", ContinentId = faerun.Id };
            db.Regions.AddRange(swordCoast, north, anauroch);
            db.SaveChanges();

            // Локации
            var baldursGate = new Location { Name = "Врата Балдура", Type = "город", Description = "Крупнейший город Побережья Мечей. Разделён на Верхний и Нижний город.", RegionId = swordCoast.Id };
            var waterdeep = new Location { Name = "Глубоководье", Type = "город", Description = "«Великий город Севера». Космополитичный мегаполис с сотнями гильдий.", RegionId = swordCoast.Id };
            var neverwinter = new Location { Name = "Невервинтер", Type = "город", Description = "«Город Умелых Рук». Известен тёплыми гаванями и искусными ремесленниками.", RegionId = swordCoast.Id };
            var candlekeep = new Location { Name = "Кэндлкип", Type = "крепость", Description = "Легендарная библиотека-крепость. Вход — только за редкую книгу.", RegionId = swordCoast.Id };
            var underdark = new Location { Name = "Подземье (входы)", Type = "подземелье", Description = "Сеть подземных тоннелей под всем Фаэруном.", RegionId = swordCoast.Id };
            var mithralHall = new Location { Name = "Мифрил Халл", Type = "крепость", Description = "Легендарная дварфийская твердыня в горах Спинхребта Мира.", RegionId = north.Id };
            var icewindDale = new Location { Name = "Долина Ледяного Ветра", Type = "регион", Description = "Продуваемые всеми ветрами земли крайнего севера.", RegionId = north.Id };
            var engrov = new Location { Name = "Руины Энгрова", Type = "руины", Description = "Останки некогда великого нетерильского города.", RegionId = anauroch.Id };
            db.Locations.AddRange(baldursGate, waterdeep, neverwinter, candlekeep, underdark, mithralHall, icewindDale, engrov);
            db.SaveChanges();

            // Фракции
            var harpers = new Faction { Name = "Харперы", Alignment = "нейтрально-добрый", Description = "Тайная организация агентов, противодействующих тирании и злу.", BaseLocationId = waterdeep.Id };
            var zhentarim = new Faction { Name = "Жентарим", Alignment = "нейтрально-злой", Description = "Торговая сеть и наёмная армия с амбициями контроля над Фаэруном.", BaseLocationId = baldursGate.Id };
            var cultDragon = new Faction { Name = "Культ Дракона", Alignment = "законно-злой", Description = "Поклоняются нежити-драконам. Стремятся возвести их на трон мира.", BaseLocationId = null };
            var gauntlet = new Faction { Name = "Орден Перчатки", Alignment = "законно-добрый", Description = "Рыцари-паладины, посвятившие себя уничтожению зла.", BaseLocationId = neverwinter.Id };
            var lordsAlliance = new Faction { Name = "Альянс Лордов", Alignment = "законно-нейтральный", Description = "Военный союз городов Побережья Мечей.", BaseLocationId = waterdeep.Id };
            var emeraldEnclave = new Faction { Name = "Изумрудный Анклав", Alignment = "нейтральный", Description = "Хранители природного баланса. Друиды и следопыты.", BaseLocationId = icewindDale.Id };
            db.Factions.AddRange(harpers, zhentarim, cultDragon, gauntlet, lordsAlliance, emeraldEnclave);
            db.SaveChanges();

            // NPC
            var laeral = new Npc { Name = "Лейрал Серебряная Рука", Race = "человек", Class = "волшебник", Level = 20, Alignment = "нейтрально-добрый", Description = "Открытый Лорд Глубоководья. Могущественный маг, бывший Хранитель Мистры.", LocationId = waterdeep.Id, FactionId = harpers.Id };
            var vararam = new Npc { Name = "Варарам Чандрасентан", Race = "человек", Class = "плут", Level = 12, Alignment = "нейтральный", Description = "Мастер гильдии Глубоководья. Контролирует преступный мир города.", LocationId = waterdeep.Id, FactionId = zhentarim.Id };
            var laezel = new Npc { Name = "Лазаэль", Race = "гитьянки", Class = "воин", Level = 10, Alignment = "законно-нейтральный", Description = "Воительница гитьянки. Резкая, прямолинейная, презирает слабость.", LocationId = waterdeep.Id, FactionId = null };
            var minsc = new Npc { Name = "Минск", Race = "человек", Class = "следопыт", Level = 8, Alignment = "хаотично-добрый", Description = "Великий воин из Рашемена с хомяком Бу.", LocationId = baldursGate.Id, FactionId = null };
            var jaheira = new Npc { Name = "Джахейра", Race = "полуэльф", Class = "друид/воин", Level = 10, Alignment = "истинно-нейтральный", Description = "Опытный друид-воин, страж Харперов.", LocationId = baldursGate.Id, FactionId = harpers.Id };
            var imoen = new Npc { Name = "Имоэн", Race = "человек", Class = "плут/маг", Level = 9, Alignment = "нейтрально-добрый", Description = "Подруга детства главного героя. Беззаботная и любопытная.", LocationId = baldursGate.Id, FactionId = null };
            var vizcount = new Npc { Name = "Виконт Лизен", Race = "человек", Class = "аристократ", Level = 5, Alignment = "законно-злой", Description = "Коррумпированный чиновник Верхнего города.", LocationId = baldursGate.Id, FactionId = zhentarim.Id };
            var neverLord = new Npc { Name = "Лорд Нейвер", Race = "человек", Class = "паладин", Level = 15, Alignment = "законно-добрый", Description = "Правитель Невервинтера. Справедливый и мудрый лидер.", LocationId = neverwinter.Id, FactionId = lordsAlliance.Id };
            var elminster = new Npc { Name = "Эльминстер Осцелт", Race = "человек", Class = "волшебник", Level = 30, Alignment = "хаотично-добрый", Description = "Легендарный Мудрец Теней. Старейший и могущественнейший маг Фаэруна.", LocationId = candlekeep.Id, FactionId = harpers.Id };
            var bruenor = new Npc { Name = "Бренор Боевой Топор", Race = "дварф", Class = "воин", Level = 16, Alignment = "законно-добрый", Description = "Король Мифрил Халла и приёмный отец Дриззта.", LocationId = mithralHall.Id, FactionId = lordsAlliance.Id };
            var drizzt = new Npc { Name = "Дриззт До'Урден", Race = "дроу", Class = "следопыт", Level = 16, Alignment = "хаотично-добрый", Description = "Изгнанник из Подземья. Непревзойдённый фехтовальщик с двумя ятаганами.", LocationId = mithralHall.Id, FactionId = null };
            var malice = new Npc { Name = "Мэт'Зик До'Урден", Race = "дроу", Class = "воин/маг", Level = 20, Alignment = "законно-злой", Description = "Матриарх дома До'Урден в Мензоберранзане. Жестокая и властолюбивая.", LocationId = underdark.Id, FactionId = null };
            db.Npcs.AddRange(laeral, vararam, laezel, minsc, jaheira, imoen, vizcount, neverLord, elminster, bruenor, drizzt, malice);
            db.SaveChanges();

            // Связи между NPC
            db.Npcrelations.AddRange(
                new Npcrelation { NpcId1 = drizzt.Id, NpcId2 = bruenor.Id, RelationType = "союзник", Description = "Дриззт и Бренор — ближайшие друзья и боевые товарищи." },
                new Npcrelation { NpcId1 = drizzt.Id, NpcId2 = malice.Id, RelationType = "враг", Description = "Дриззт бежал из Подземья, спасаясь от матери." },
                new Npcrelation { NpcId1 = minsc.Id, NpcId2 = jaheira.Id, RelationType = "союзник", Description = "Минск и Джахейра — товарищи по многим приключениям." },
                new Npcrelation { NpcId1 = minsc.Id, NpcId2 = imoen.Id, RelationType = "друг", Description = "Минск обожает Имоэн. Называет её «маленькой ведьмочкой»." },
                new Npcrelation { NpcId1 = laeral.Id, NpcId2 = elminster.Id, RelationType = "союзник", Description = "Лейрал и Эльминстер — старые коллеги, оба служили Мистре." },
                new Npcrelation { NpcId1 = vizcount.Id, NpcId2 = vararam.Id, RelationType = "союзник", Description = "Виконт тайно передаёт информацию Жентариму." }
            );
            db.SaveChanges();

            // Квесты
            var quest1 = new Quest { Title = "Тайна железного трона", Description = "Торговый союз «Железный Трон» наводняет Побережье Мечей отравленными товарами.", Status = "активный", LocationId = baldursGate.Id, QuestGiverId = jaheira.Id };
            var quest2 = new Quest { Title = "Похищение из Кэндлкипа", Description = "Из хранилища Кэндлкипа пропали три пророческих свитка Овена.", Status = "активный", LocationId = candlekeep.Id, QuestGiverId = elminster.Id };
            var quest3 = new Quest { Title = "Осада Мифрил Халла", Description = "Орды орков движутся к Мифрил Халлу.", Status = "активный", LocationId = mithralHall.Id, QuestGiverId = bruenor.Id };
            var quest4 = new Quest { Title = "Личинка в голове", Description = "Группа авантюристов заражена личинками иллитидов.", Status = "активный", LocationId = waterdeep.Id, QuestGiverId = laezel.Id };
            var quest5 = new Quest { Title = "Падение дома До'Урден", Description = "Дриззт получил весть из Подземья: его сестра пытается свергнуть Мэт'Зик.", Status = "завершён", LocationId = underdark.Id, QuestGiverId = drizzt.Id };
            var quest6 = new Quest { Title = "Коррупция в Верхнем городе", Description = "Джахейра подозревает Виконта Лизена в связях с Жентаримом.", Status = "активный", LocationId = baldursGate.Id, QuestGiverId = jaheira.Id };
            db.Quests.AddRange(quest1, quest2, quest3, quest4, quest5, quest6);
            db.SaveChanges();

            // QuestNPC
            db.QuestNpcs.AddRange(
                new QuestNpc { QuestId = quest1.Id, NpcId = minsc.Id, Role = "участник" },
                new QuestNpc { QuestId = quest1.Id, NpcId = imoen.Id, Role = "участник" },
                new QuestNpc { QuestId = quest1.Id, NpcId = vizcount.Id, Role = "цель" },
                new QuestNpc { QuestId = quest2.Id, NpcId = elminster.Id, Role = "наградодатель" },
                new QuestNpc { QuestId = quest2.Id, NpcId = laeral.Id, Role = "союзник" },
                new QuestNpc { QuestId = quest3.Id, NpcId = bruenor.Id, Role = "наградодатель" },
                new QuestNpc { QuestId = quest3.Id, NpcId = drizzt.Id, Role = "участник" },
                new QuestNpc { QuestId = quest4.Id, NpcId = laezel.Id, Role = "участник" },
                new QuestNpc { QuestId = quest4.Id, NpcId = laeral.Id, Role = "союзник" },
                new QuestNpc { QuestId = quest5.Id, NpcId = drizzt.Id, Role = "участник" },
                new QuestNpc { QuestId = quest5.Id, NpcId = malice.Id, Role = "цель" },
                new QuestNpc { QuestId = quest6.Id, NpcId = jaheira.Id, Role = "наградодатель" },
                new QuestNpc { QuestId = quest6.Id, NpcId = vizcount.Id, Role = "цель" }
            );
            db.SaveChanges();

            // Заметки
            db.Notes.AddRange(
                new Note { Title = "Секрет Лазаэль", Content = "Лазаэль скрывает, что уже пыталась связаться с гитьянкийским крейсером. Не доверять ей безоговорочно.", CreatedAt = DateTime.Now, LinkedType = "NPC", LinkedId = laezel.Id },
                new Note { Title = "Подземные тоннели под Вратами Балдура", Content = "По слухам, под городом есть старые тоннели. Возможный путь в штаб Железного Трона.", CreatedAt = DateTime.Now, LinkedType = "Location", LinkedId = baldursGate.Id },
                new Note { Title = "Культ Дракона — агент в Кэндлкипе", Content = "Один из переписчиков подозрительно часто задерживается в запретном крыле.", CreatedAt = DateTime.Now, LinkedType = "Faction", LinkedId = cultDragon.Id },
                new Note { Title = "Эльминстер знает больше, чем говорит", Content = "В разговоре о пророчестве сделал паузу при упоминании «Имени без Слова».", CreatedAt = DateTime.Now, LinkedType = "NPC", LinkedId = elminster.Id },
                new Note { Title = "Слабость армии орков", Content = "Шаман Хрок держит армию с помощью Барабана Грааля. Уничтожить барабан — армия рассыплется.", CreatedAt = DateTime.Now, LinkedType = "Quest", LinkedId = quest3.Id },
                new Note { Title = "Явки Жентарима", Content = "Таверна «Три огня», склад у восточных ворот, ювелирная лавка «Золотой крот».", CreatedAt = DateTime.Now, LinkedType = "Faction", LinkedId = zhentarim.Id }
            );
            db.SaveChanges();

            // Закреплённые NPC на Dashboard
            db.PinnedNpcs.AddRange(
                new PinnedNpc { NpcId = drizzt.Id, Slot = 0 },
                new PinnedNpc { NpcId = minsc.Id, Slot = 1 },
                new PinnedNpc { NpcId = elminster.Id, Slot = 2 },
                new PinnedNpc { NpcId = laeral.Id, Slot = 3 }
            );
            db.SaveChanges();
        }
    }
}