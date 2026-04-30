import { NextRequest, NextResponse } from 'next/server';

const IMMUTABLE_CACHE_CONTROL = 'public, max-age=31536000, immutable';
const ALLOWED_IMAGE_HOSTS = new Set([
  'img.tarotnow.xyz',
  'media.tarotnow.xyz',
]);

function isAllowedHost(hostname: string): boolean {
  return ALLOWED_IMAGE_HOSTS.has(hostname.toLowerCase());
}

function resolveSourceUrl(rawSource: string, request: NextRequest): URL | null {
  try {
    const parsed = new URL(rawSource);
    return parsed;
  } catch {
    if (!rawSource.startsWith('/')) {
      return null;
    }

    try {
      return new URL(rawSource, request.nextUrl.origin);
    } catch {
      return null;
    }
  }
}

export async function GET(request: NextRequest): Promise<NextResponse> {
  const source = request.nextUrl.searchParams.get('src');
  const version = request.nextUrl.searchParams.get('iv') ?? '';

  if (!source) {
    return NextResponse.json({ error: 'Missing src query parameter.' }, { status: 400 });
  }

  const sourceUrl = resolveSourceUrl(source, request);
  if (!sourceUrl || !isAllowedHost(sourceUrl.hostname)) {
    return NextResponse.json({ error: 'Unsupported image host.' }, { status: 400 });
  }

  let upstreamResponse: Response;
  try {
    upstreamResponse = await fetch(sourceUrl.toString(), {
      method: 'GET',
      cache: 'force-cache',
      next: { revalidate: 31536000 },
    });
  } catch {
    return NextResponse.json({ error: 'Failed to fetch upstream image.' }, { status: 502 });
  }

  if (!upstreamResponse.ok || !upstreamResponse.body) {
    return NextResponse.json({ error: 'Upstream image unavailable.' }, { status: 502 });
  }

  const headers = new Headers();
  headers.set('Cache-Control', IMMUTABLE_CACHE_CONTROL);
  headers.set('Content-Type', upstreamResponse.headers.get('content-type') ?? 'application/octet-stream');
  const contentLength = upstreamResponse.headers.get('content-length');
  if (contentLength) {
    headers.set('Content-Length', contentLength);
  }
  const etag = upstreamResponse.headers.get('etag');
  if (etag) {
    headers.set('ETag', etag);
  } else if (version) {
    headers.set('ETag', `"${version}"`);
  }

  return new NextResponse(upstreamResponse.body, {
    status: 200,
    headers,
  });
}
