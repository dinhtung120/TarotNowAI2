const urls = [
  'https://www.tarotnow.xyz/',
  'https://www.tarotnow.xyz/themes/prismatic-royal.css',
  'https://www.tarotnow.xyz/_next/static/chunks/main-app.js',
  'https://media.tarotnow.xyz/community/2ef125e575c84b8d990e4c90ed64d5f3.webp',
  'https://img.tarotnow.xyz/light-god-50/04_The_Empress_50_20260325_181348.avif?iv=81a3d9698977fda2&variant=thumb',
];

for (const url of urls) {
  try {
    const response = await fetch(url, { method: 'HEAD' });
    console.log(JSON.stringify({
      url,
      status: response.status,
      cacheControl: response.headers.get('cache-control'),
      cfCacheStatus: response.headers.get('cf-cache-status'),
      contentType: response.headers.get('content-type'),
      contentLength: response.headers.get('content-length'),
      etag: response.headers.get('etag'),
      serverTiming: response.headers.get('server-timing'),
    }));
  } catch (error) {
    console.log(JSON.stringify({ url, error: error instanceof Error ? error.message : String(error) }));
  }
}
