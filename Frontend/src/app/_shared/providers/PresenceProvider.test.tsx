import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

const usePathname = vi.fn();
const usePresenceConnection = vi.fn();

vi.mock('@/i18n/routing', () => ({
  usePathname: () => usePathname(),
  routing: {
    locales: ['vi', 'en', 'zh'],
  },
}));

vi.mock('@/app/_shared/hooks/usePresenceConnection', () => ({
  usePresenceConnection: (options: { enabled?: boolean }) => usePresenceConnection(options),
}));

import PresenceProvider from './PresenceProvider';

describe('PresenceProvider', () => {
  let container: HTMLDivElement;
  let root: Root;

  beforeEach(() => {
    container = document.createElement('div');
    document.body.appendChild(container);
    root = createRoot(container);
    usePathname.mockReset();
    usePresenceConnection.mockReset();
  });

  afterEach(() => {
    act(() => root.unmount());
    container.remove();
  });

  it.each(['/vi/login', '/vi/register', '/vi/forgot-password'])('disables presence on auth-public route %s', (pathname) => {
    usePathname.mockReturnValue(pathname);

    act(() => {
      root.render(<PresenceProvider><div>child</div></PresenceProvider>);
    });

    expect(usePresenceConnection).toHaveBeenCalledWith({ enabled: false });
  });

  it.each(['/vi', '/vi/profile', '/vi/chat'])('enables presence-capable routes for %s', (pathname) => {
    usePathname.mockReturnValue(pathname);

    act(() => {
      root.render(<PresenceProvider><div>child</div></PresenceProvider>);
    });

    expect(usePresenceConnection).toHaveBeenCalledWith({ enabled: true });
  });

  it.each(['/vi/admin', '/vi/legal/tos'])('disables presence on non-user route %s', (pathname) => {
    usePathname.mockReturnValue(pathname);

    act(() => {
      root.render(<PresenceProvider><div>child</div></PresenceProvider>);
    });

    expect(usePresenceConnection).toHaveBeenCalledWith({ enabled: false });
  });
});
