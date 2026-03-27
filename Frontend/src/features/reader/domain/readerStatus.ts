export type ReaderStatus = 'accepting_questions' | 'online' | 'away' | 'offline';

export function normalizeReaderStatus(status?: string | null): ReaderStatus {
 const value = status?.trim().toLowerCase();
 switch (value) {
  case 'accepting_questions':
  case 'acceptingquestions':
  case 'accepting-questions':
  case 'accepting':
  case 'ready':
  case 'available':
   return 'accepting_questions';
  case 'online':
  case 'active':
  case 'connected':
   return 'online';
  case 'away':
  case 'busy':
  case 'idle':
   return 'away';
  case 'offline':
  case 'disconnected':
  case 'invisible':
   return 'offline';
  default:
   return 'offline';
 }
}
