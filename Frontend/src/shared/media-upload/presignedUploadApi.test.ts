import { beforeEach, describe, expect, it, vi } from 'vitest';
import { postJsonToApiV1 } from '@/shared/http/clientJsonRequest';
import {
 confirmAvatarUpload,
 confirmCommunityImageUpload,
 presignAvatarUpload,
 presignCommunityImageUpload,
 presignConversationMediaUpload,
} from '@/shared/media-upload/presignedUploadApi';

vi.mock('@/shared/http/clientJsonRequest', () => ({
 postJsonToApiV1: vi.fn(),
}));

const mockedPostJsonToApiV1 = vi.mocked(postJsonToApiV1);

describe('presignedUploadApi', () => {
 beforeEach(() => {
  vi.clearAllMocks();
  mockedPostJsonToApiV1.mockResolvedValue({ ok: true, data: { uploadUrl: 'https://upload', objectKey: 'obj', publicUrl: 'https://cdn', uploadToken: 'token' } });
 });

 it('calls presign avatar endpoint with fixed image/webp content type', async () => {
  await presignAvatarUpload(1024);

  expect(mockedPostJsonToApiV1).toHaveBeenCalledWith(
   '/profile/avatar/presign',
   { contentType: 'image/webp', sizeBytes: 1024 },
   expect.objectContaining({ fallbackErrorMessage: 'Không thể tạo URL upload avatar.' }),
  );
 });

 it('calls confirm avatar endpoint with payload passthrough', async () => {
  const payload = { objectKey: 'obj', publicUrl: 'https://cdn', uploadToken: 'tok' };

  await confirmAvatarUpload(payload);

  expect(mockedPostJsonToApiV1).toHaveBeenCalledWith('/profile/avatar/confirm', payload, expect.any(Object));
 });

 it('calls community image endpoints with context payload', async () => {
  await presignCommunityImageUpload({
   contextDraftId: 'draft-1',
   contextType: 'post',
   sizeBytes: 2000,
  });

  await confirmCommunityImageUpload({
   contextDraftId: 'draft-1',
   contextType: 'post',
   objectKey: 'obj',
   publicUrl: 'https://cdn',
   uploadToken: 'tok',
  });

  expect(mockedPostJsonToApiV1).toHaveBeenNthCalledWith(
   1,
   '/community/image/presign',
   expect.objectContaining({ contextDraftId: 'draft-1', contextType: 'post', contentType: 'image/webp' }),
   expect.any(Object),
  );
  expect(mockedPostJsonToApiV1).toHaveBeenNthCalledWith(
   2,
   '/community/image/confirm',
   expect.objectContaining({ contextDraftId: 'draft-1', contextType: 'post', objectKey: 'obj' }),
   expect.any(Object),
  );
 });

 it('calls conversation media presign endpoint with conversation id path', async () => {
  await presignConversationMediaUpload({
   conversationId: 'conv-1',
   contentType: 'audio/webm',
   mediaKind: 'voice',
   sizeBytes: 1234,
   durationMs: 5678,
  });

  expect(mockedPostJsonToApiV1).toHaveBeenCalledWith(
   '/conversations/conv-1/media/presign',
   expect.objectContaining({ contentType: 'audio/webm', mediaKind: 'voice', sizeBytes: 1234, durationMs: 5678 }),
   expect.any(Object),
  );
 });
});
