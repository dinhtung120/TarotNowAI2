import path from 'node:path';
import { defineConfig } from 'vitest/config';

export default defineConfig({
 resolve: {
  alias: {
   '@': path.resolve(__dirname, './src'),
  },
 },
 test: {
  environment: 'jsdom',
  setupFiles: ['./src/test/setup.ts'],
  include: ['src/**/*.test.{ts,tsx}', 'scripts/**/*.test.ts'],
  coverage: {
   provider: 'v8',
   reporter: ['text', 'html', 'json-summary'],
   exclude: [
    'src/shared/media-upload/index.ts',
   ],
   thresholds: {
    statements: 70,
    branches: 55,
    functions: 75,
    lines: 72,
    'src/shared/infrastructure/http/clientFetch.ts': {
     statements: 75,
     branches: 65,
    },
    'src/shared/infrastructure/http/clientJsonRequest.ts': {
     statements: 90,
     branches: 80,
    },
    'src/features/wallet/deposit/actions/user-orders.ts': {
     statements: 75,
     branches: 60,
    },
    'src/shared/infrastructure/auth/serverAuth.ts': {
     statements: 70,
     branches: 55,
    },
    'src/shared/infrastructure/auth/deviceId.ts': {
     statements: 80,
     branches: 75,
    },
    'src/shared/infrastructure/auth/refreshClient.ts': {
     statements: 90,
     branches: 80,
    },
    'src/shared/infrastructure/http/apiUrl.ts': {
     statements: 75,
     branches: 50,
    },
    'src/shared/infrastructure/http/serverHttpClient.ts': {
     statements: 75,
     branches: 60,
    },
    'src/shared/media-upload/presignedUploadApi.ts': {
     statements: 70,
     branches: 50,
    },
    'src/shared/media-upload/uploadViaXhr.ts': {
     statements: 70,
     branches: 50,
    },
    'src/shared/media-upload/uploadWithRetry.ts': {
     statements: 70,
     branches: 50,
    },
   },
  },
 },
});
