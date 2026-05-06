export type ReaderStatus = 'online' | 'busy' | 'offline';

export function normalizeReaderStatus(status?: string | null): ReaderStatus {
 const value = status?.trim().toLowerCase();
 switch (value) {
  case 'online':
  case 'active':
  case 'connected':
   return 'online';
  
  case 'busy':
  case 'away':
  case 'idle':
  case 'accepting_questions':
  case 'acceptingquestions':
  case 'accepting-questions':
  case 'accepting':
  case 'ready':
  case 'available':
   return 'busy';
   
  case 'offline':
  case 'disconnected':
  case 'invisible':
  default:
   return 'offline';
 }
}
