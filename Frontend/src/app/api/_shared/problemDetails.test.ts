import { describe, expect, it } from 'vitest';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';

describe('buildProblemResponse', () => {
 it.each([
  [400, 'Bad Request'],
  [401, 'Unauthorized'],
  [403, 'Forbidden'],
  [404, 'Not Found'],
  [409, 'Conflict'],
  [422, 'Unprocessable Entity'],
  [429, 'Too Many Requests'],
  [500, 'Internal Server Error'],
  [418, 'Request Failed'],
 ])('returns RFC7807 content type and the expected title for %s', async (status, title) => {
  const response = buildProblemResponse({
   status,
   detail: `Status ${status}.`,
  });

  expect(response.status).toBe(status);
  expect(response.headers.get('Content-Type')).toBe('application/problem+json');
  await expect(response.json()).resolves.toMatchObject({
   type: 'about:blank',
   title,
   status,
   detail: `Status ${status}.`,
  });
 });

 it('supports the legacy positional signature', async () => {
  const response = buildProblemResponse(403, 'Forbidden.', 'ACCESS_DENIED');

  await expect(response.json()).resolves.toMatchObject({
   title: 'Forbidden',
   status: 403,
   detail: 'Forbidden.',
   errorCode: 'ACCESS_DENIED',
  });
 });

 it('preserves explicit title and type overrides', async () => {
  const response = buildProblemResponse({
   status: 400,
   detail: 'Custom detail.',
   title: 'Custom Title',
   type: 'https://example.test/errors/custom',
  });

  await expect(response.json()).resolves.toMatchObject({
   title: 'Custom Title',
   type: 'https://example.test/errors/custom',
  });
 });
});
