# Trek & Hike Tracker - Frontend (Angular)

Beautiful and engaging Angular frontend for the Trek & Hike Tracker application.

## Features

✨ **Beautiful Design**
- Modern, responsive UI with Tailwind CSS
- Green nature-inspired color scheme
- Engaging landing page that encourages exploration

🎯 **User Experience**
- Seamless authentication (Register/Login)
- Route discovery and exploration
- Real-time API integration
- Mobile-friendly design

🔧 **Technology Stack**
- Angular 22
- TypeScript
- Tailwind CSS
- RxJS for reactive programming
- Standalone Components (latest Angular pattern)

## Quick Start

### Prerequisites
- Node.js 18+ and npm installed
- ASP.NET Core API running on http://localhost:5245

### Installation

```bash
cd trek-hike-tracker-web
npm install
```

### Development Server

```bash
npm start
```

The app will open automatically at http://localhost:4200

API requests are automatically proxied to http://localhost:5245 (see `proxy.conf.json`)

### Building for Production

```bash
npm run build:prod
```

Built files will be in `dist/trek-hike-tracker-web/browser/`

## Project Structure

```
src/
├── app/
│   ├── pages/                 # Page components
│   │   ├── home.component.ts      # Beautiful landing page
│   │   ├── login.component.ts     # Login form
│   │   ├── register.component.ts  # Registration form
│   │   ├── routes.component.ts    # Routes list
│   │   ├── route-detail.component.ts
│   │   └── profile.component.ts
│   ├── services/              # API services
│   │   ├── auth.service.ts        # Authentication
│   │   └── route.service.ts       # Routes API
│   ├── interceptors/          # HTTP interceptors
│   │   └── auth.interceptor.ts    # JWT token injection
│   ├── app.ts                 # Root component
│   └── app.routes.ts          # Routing configuration
├── environments/              # Environment configs
├── styles.scss                # Global styles (Tailwind)
└── main.ts                    # Application entry point
```

## API Integration

The frontend communicates with the backend API:

- **Login**: POST `/api/auth/login`
- **Register**: POST `/api/auth/register`
- **Get Routes**: GET `/api/routes?page=1&pageSize=10`
- **Like Route**: POST `/api/routes/{id}/social/like`
- **Add Comment**: POST `/api/routes/{id}/social/comments`

JWT tokens are automatically added to all requests via the auth interceptor.

## Styling

The app uses Tailwind CSS for styling. Customize colors in `tailwind.config.js`:

```js
colors: {
  primary: '#2D7A4A',   // Main green
  secondary: '#E8A87C', // Orange
  accent: '#8BC34A',    // Light green
}
```

## Running Frontend + Backend Together

Use the startup script at the project root:

```bash
.\start-dev.ps1
```

This will:
1. Start ASP.NET Core API on port 5245
2. Start Angular dev server on port 4200
3. Automatically open the browser

## Future Enhancements

- [ ] Routes list with advanced filtering
- [ ] Route detail page with map
- [ ] User profile page
- [ ] Create/edit route forms
- [ ] Photo gallery
- [ ] Real-time notifications
- [ ] Dark mode toggle

## Notes

- The frontend is fully standalone (no AppModule)
- Uses Angular's latest component architecture
- Lazy-loaded routes for better performance
- Type-safe HTTP client with strong typing
