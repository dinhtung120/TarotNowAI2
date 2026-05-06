import { describe, expect, it, vi, beforeEach } from 'vitest';
import { NextRequest } from 'next/server';
import { GET } from '@/app/api/readers/route';
import { listReaders } from '@/features/reader/shared';

vi.mock('@/features/reader/shared', () => ({
 listReaders: vi.fn(),
}));

const mockedListReaders = vi.mocked(listReaders);

describe('GET /api/readers', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('maps action failure status from ActionResult metadata', async () => {
  mockedListReaders.mockResolvedValue({
   success: false,
   error: 'upstream unavailable',
   status: 503,
  });

  const request = new NextRequest('http://localhost/api/readers?page=1&pageSize=12');
  const response = await GET(request);

  expect(response.status).toBe(503);
  const payload = await response.json();
  expect(payload.detail).toBe('upstream unavailable');
 });
});
