import { beforeEach, describe, expect, it, vi } from 'vitest';
import { NextRequest } from 'next/server';
import { GET } from '@/app/api/readers/[id]/route';
import { getReaderProfile } from '@/features/reader/shared';

vi.mock('@/features/reader/shared', () => ({
 getReaderProfile: vi.fn(),
}));

const mockedGetReaderProfile = vi.mocked(getReaderProfile);

describe('GET /api/readers/[id]', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('maps failure to status from ActionResult metadata', async () => {
  mockedGetReaderProfile.mockResolvedValue({
   success: false,
   error: 'backend timeout',
   status: 503,
  });

  const request = new NextRequest('http://localhost/api/readers/reader-1');
  const response = await GET(request, { params: Promise.resolve({ id: 'reader-1' }) });

  expect(response.status).toBe(503);
  const payload = await response.json();
  expect(payload.detail).toBe('backend timeout');
 });
});
