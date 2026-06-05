# Trek & Hike Tracker — Texnik Topshiriq (TZ)

---

## 1. Loyiha haqida

### Nima bu?

Trek & Hike Tracker — Uzbekiston va dunyo bo'ylab hiking va trekking marshrut larini qo'shish, ko'rish, reyting berish va ulashish uchun platforma. Foydalanuvchilar o'zlari bosib o'tgan yo'llarni, fotolarni, qiyinlik darajasini va tavsiflarini qo'shadi. Boshqalar esa bu ma'lumotlardan safar rejalashtirish uchun foydalanadi.

### Nega kerak? (Real muammo)

Hozir Uzbekistonda Zaamin, Chimgan, Beldersoy, Nuratau kabi joylar uchun to'liq, ishonchli marshrut ma'lumotlari yo'q. Odamlar Telegram guruhlari yoki og'zaki gaplashib marshrut topadi. Bu loyiha shu bo'shliqni to'ldiradi — va bu portfolioda "men o'zim ham ishlataman" deb aytish mumkin bo'lgan real product.

### Portfolio uchun nima beradi?

- To'liq Full Stack ko'rinadi (Angular + .NET)
- Clean Architecture + CQRS/MediatR — arxitektura saviyasi
- MinIO, RabbitMQ, Hangfire — production darajasi texnologiyalar
- Docker + CI/CD — deploy madaniyati
- Real use case + o'z hobby bilan bog'liq — recruiter uchun yodda qoladi

---

## 2. Foydalanuvchilar (Roles)

**Anonymous (ro'yxatdan o'tmagan):**
- Marshrut larni ko'rish, qidirish, filter qilish
- Fotolarni ko'rish

**User (ro'yxatdan o'tgan):**
- O'z marshrut qo'shish, tahrirlash, o'chirish
- Foto yuklash (max 10 ta / marshrut)
- Like, comment, reyting berish
- Sevimlilariga saqlash
- Profil sahifasi

**Admin:**
- Barcha marshrut lar va commentlarni moderatsiya qilish
- Foydalanuvchilarni boshqarish
- Dashboard: statistika, hisobotlar

---

## 3. Funksionallik (Features)

### 3.1 Auth moduli
- Ro'yxatdan o'tish (email + password)
- Kirish (JWT access token + refresh token)
- Parolni tiklash (email orqali)

### 3.2 Marshrut moduli (Route)
Har bir marshrut quyidagilarni saqlaydi:
- Nomi, tavsifi (uz/ru/en)
- Boshlanish va tugash nuqtasi (koordinata)
- Qiyinlik darajasi: Oson / O'rta / Qiyin / Ekstremal
- Masofa (km), balandlik farqi (m)
- Taxminiy vaqt (soat)
- Mavsum (yil bo'yi / faqat yozda / qishda)
- Region (Toshkent viloyati, Samarqand, Navoiy…)
- Tags: #waterfall, #glacier, #camping va h.k.

### 3.3 Foto moduli (MinIO)
- Foydalanuvchi marshrut uchun foto yuklaydi
- Max 10 ta foto, har biri max 10 MB
- MinIO'da saqlash, preview thumbnail avtomatik generatsiya
- Asosiy foto (cover image) tanlash

### 3.4 Social moduli
- Like (marshrut va commentga)
- Comment (nested — javob yozish mumkin)
- Reyting (1–5 yulduz, faqat bir marta)
- Bookmark (sevimlilarga saqlash)

### 3.5 Qidiruv va Filter
- Qidiruv: nom, tag, region bo'yicha
- Filter: qiyinlik, masofa, mavsum, reyting
- Sort: eng mashhur, eng yangi, eng yaxshi reyting

### 3.6 Notification moduli (RabbitMQ + SignalR)
- Marshrut ingizga like/comment kelganda real-time bildirishnoma
- Email bildirishnoma (ixtiyoriy)
- Havola: producer marshrut'da event publish qiladi → consumer xabar yuboradi

### 3.7 Background Jobs (Hangfire)
- **Haftalik top marshrut** — har dushanba yangi haftaning eng mashhur 10 ta marshrut i hisoblanadi va email/notification yuboriladi
- **Rasm tozalash** — o'chirilgan marshrut larning MinIO'dagi fotolari tozalanadi (ResolveMarkedFiles pattern)
- **Statistika refresh** — dashboard materialized view'lar yangilanadi

### 3.8 Admin Dashboard
- Jami foydalanuvchilar, marshrut lar, commentlar soni
- Oylik yangi a'zolar grafigi
- Eng mashhur marshrut lar
- Moderatsiya: shikoyat qilingan contentni ko'rish va o'chirish
- Excel eksport (EPPlus)

---

## 4. Texnik Stack

| Qatlam | Texnologiya |
|--------|-------------|
| Backend | ASP.NET Core 8 |
| Arxitektura | Clean Architecture + CQRS/MediatR |
| ORM | Entity Framework Core 8 |
| DB | PostgreSQL 16 |
| Fayl saqlash | MinIO |
| Message broker | RabbitMQ |
| Background jobs | Hangfire |
| Cache | Redis |
| Real-time | SignalR |
| Excel | EPPlus |
| Auth | JWT + Refresh Token |
| Frontend | Angular 17+ |
| UI | Angular Material / Tailwind |
| Map | Leaflet.js (OpenStreetMap) |
| Deploy | Docker + Docker Compose |
| CI/CD | GitHub Actions (yoki Jenkins) |
| API docs | Swagger/OpenAPI |

---

## 5. Arxitektura

```
TrekTracker.sln
├── TrekTracker.API            ← Controllers, Middleware, Program.cs
├── TrekTracker.Application    ← CQRS Commands/Queries, DTOs, Interfaces
├── TrekTracker.Domain         ← Entities, Enums, Domain Events
├── TrekTracker.Infrastructure ← EF Core, MinIO, RabbitMQ, Hangfire, Redis
└── TrekTracker.Angular        ← Angular frontend (yoki alohida repo)
```

### CQRS misoli (Route yaratish):

```csharp
// Command
public record CreateRouteCommand(
    string Name,
    string Description,
    double StartLat, double StartLng,
    double EndLat, double EndLng,
    DifficultyLevel Difficulty,
    double DistanceKm,
    int EstimatedHours,
    List<string> Tags
) : IRequest<int>;

// Handler
public class CreateRouteCommandHandler : IRequestHandler<CreateRouteCommand, int>
{
    public async Task<int> Handle(CreateRouteCommand request, CancellationToken ct)
    {
        var route = new Route { ... };
        await _context.Routes.AddAsync(route, ct);
        await _context.SaveChangesAsync(ct);

        // RabbitMQ orqali event publish
        await _publisher.PublishAsync(new RouteCreatedEvent(route.Id));

        return route.Id;
    }
}
```

---

## 6. Ma'lumotlar bazasi (asosiy jadvallar)

```sql
-- Foydalanuvchi
users (id, email, password_hash, username, avatar_url, bio, created_at)

-- Marshrut
routes (id, user_id, name, description, difficulty, distance_km,
        estimated_hours, start_lat, start_lng, end_lat, end_lng,
        season, region_id, is_active, created_at)

-- Fotolar
route_photos (id, route_id, file_path, is_cover, uploaded_at)

-- Tags
tags (id, name)
route_tags (route_id, tag_id)

-- Social
route_likes (user_id, route_id, created_at)
route_ratings (user_id, route_id, value, created_at)
route_bookmarks (user_id, route_id)
comments (id, route_id, user_id, parent_id, text, created_at)
comment_likes (user_id, comment_id)

-- Bildirishnomalar
notifications (id, user_id, type, payload, is_read, created_at)
```

---

## 7. Asosiy API Endpointlar

```
Auth:
POST   /api/auth/register
POST   /api/auth/login
POST   /api/auth/refresh

Routes:
GET    /api/routes              ← list + filter + pagination
GET    /api/routes/{id}         ← detail
POST   /api/routes              ← [Auth] create
PUT    /api/routes/{id}         ← [Auth] update
DELETE /api/routes/{id}         ← [Auth] delete

Photos:
POST   /api/routes/{id}/photos  ← [Auth] upload (multipart)
DELETE /api/photos/{id}         ← [Auth]

Social:
POST   /api/routes/{id}/like
POST   /api/routes/{id}/rate    ← { value: 1-5 }
POST   /api/routes/{id}/bookmark
GET    /api/routes/{id}/comments
POST   /api/routes/{id}/comments

Admin:
GET    /api/admin/dashboard
GET    /api/admin/routes        ← all with moderation
DELETE /api/admin/routes/{id}
GET    /api/admin/reports/excel ← EPPlus
```

---

## 8. Rivojlanish bosqichlari (Timeline)

### Hafta 1–2: Poydevor
- [ ] Solution struktura (Clean Architecture)
- [ ] DB schema + EF Core migrations
- [ ] JWT Auth (register/login/refresh)
- [ ] Route CRUD (CQRS/MediatR)

### Hafta 3–4: Asosiy funksionallik
- [ ] MinIO integration (foto yuklash)
- [ ] Filter/qidiruv/pagination
- [ ] Social: like, rating, bookmark
- [ ] Comments (nested)

### Hafta 5–6: Advanced
- [ ] RabbitMQ + SignalR notifications
- [ ] Hangfire background jobs
- [ ] Redis caching (top routes)
- [ ] Admin dashboard + EPPlus Excel

### Hafta 7–8: Frontend + Deploy
- [ ] Angular: auth, route list, detail, map (Leaflet)
- [ ] Angular: profil, bookmark, notifications
- [ ] Docker Compose (api + db + minio + rabbitmq + redis)
- [ ] GitHub Actions CI/CD
- [ ] Swagger to'liq to'ldirish
- [ ] README yozish (portfolio uchun muhim!)

---

## 9. Portfolio uchun README'da nima yozish kerak

1. Loyiha haqida qisqa (2-3 gap)
2. Features ro'yxati (screenshotlar bilan)
3. Arxitektura diagrammasi
4. Qanday ishga tushirish (Docker bilan 1 komanda)
5. Texnologiyalar (badges bilan)
6. "Why I built this" — shaxsiy hikoya (Zaamin, Chimgan…)

---

*TZ versiyasi: 1.0 | Muallif: Islombek | Sana: 2026-06*
