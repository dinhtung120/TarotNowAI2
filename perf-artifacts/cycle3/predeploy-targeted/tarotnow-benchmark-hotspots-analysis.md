# TarotNow Benchmark Analysis

- Run time (UTC): 2026-04-30T06:15:45.063Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 11 | 52.1 | 4322 | 0 | 1 | 0 | 3 | 2 | yes |
| logged-in-reader | desktop | 10 | 53.9 | 3911 | 16 | 1 | 0 | 3 | 3 | yes |
| logged-out | mobile | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 42.9 | 5646 | 0 | 1 | 0 | 4 | 2 | yes |
| logged-in-reader | mobile | 9 | 43.7 | 4111 | 0 | 1 | 0 | 3 | 2 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 35.0 | 3775 | 0 |
| logged-in-admin | desktop | collection | 1 | 210.0 | 14908 | 0 |
| logged-in-admin | desktop | community | 1 | 43.0 | 3968 | 0 |
| logged-in-admin | desktop | readers | 3 | 33.0 | 3112 | 0 |
| logged-in-admin | desktop | reading | 4 | 38.3 | 3149 | 0 |
| logged-in-admin | desktop | wallet | 1 | 33.0 | 2963 | 0 |
| logged-in-admin | mobile | admin | 1 | 35.0 | 3516 | 0 |
| logged-in-admin | mobile | collection | 1 | 106.0 | 7925 | 0 |
| logged-in-admin | mobile | community | 1 | 43.0 | 3856 | 0 |
| logged-in-admin | mobile | readers | 3 | 33.7 | 9589 | 0 |
| logged-in-admin | mobile | reading | 3 | 37.0 | 3137 | 0 |
| logged-in-admin | mobile | wallet | 1 | 33.0 | 2988 | 0 |
| logged-in-reader | desktop | collection | 1 | 211.0 | 10265 | 16 |
| logged-in-reader | desktop | community | 1 | 44.0 | 4099 | 0 |
| logged-in-reader | desktop | readers | 3 | 33.0 | 2965 | 0 |
| logged-in-reader | desktop | reading | 4 | 38.0 | 3167 | 0 |
| logged-in-reader | desktop | wallet | 1 | 33.0 | 3182 | 0 |
| logged-in-reader | mobile | collection | 1 | 106.0 | 7916 | 0 |
| logged-in-reader | mobile | community | 1 | 43.0 | 6381 | 0 |
| logged-in-reader | mobile | readers | 3 | 33.3 | 3062 | 0 |
| logged-in-reader | mobile | reading | 3 | 37.0 | 3275 | 0 |
| logged-in-reader | mobile | wallet | 1 | 33.0 | 3691 | 0 |

## Key Findings
1. Critical: 22 page(s) vượt 35 requests.
2. High: 40 page(s) vượt 25 requests.
3. High: 415 request vượt 800ms.
4. Medium: 270 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 299 image request(s) >800ms trên 360 image request(s).

## Top Slow Pages
| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | 22663 | 34 | 1548 | 376.0 | 0.0030 |
| logged-in-admin | desktop | /vi/collection | 14908 | 210 | 3484 | 859.0 | 0.0029 |
| logged-in-reader | desktop | /vi/collection | 10265 | 211 | 2400 | 1088.0 | 0.0026 |
| logged-in-admin | mobile | /vi/collection | 7925 | 106 | 1368 | 132.0 | 0.0030 |
| logged-in-reader | mobile | /vi/collection | 7916 | 106 | 1568 | 242.0 | 0.0029 |
| logged-in-reader | mobile | /vi/community | 6381 | 43 | 3396 | 216.0 | 0.0051 |
| logged-in-reader | desktop | /vi/community | 4099 | 44 | 2060 | 56.0 | 0.0026 |
| logged-in-admin | desktop | /vi/community | 3968 | 43 | 2180 | 40.0 | 0.0029 |
| logged-in-admin | mobile | /vi/community | 3856 | 43 | 1948 | 39.0 | 0.0000 |
| logged-in-admin | desktop | /vi/admin | 3775 | 35 | 1488 | 58.0 | 0.0000 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18747 | 91 | https://www.tarotnow.xyz/_next/static/chunks/12gvu2nm._6pa.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18697 | 78 | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18696 | 71 | https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18516 | 76 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18450 | 83 | https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 18441 | 323 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17816 | 73 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17766 | 76 | https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17577 | 76 | https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17565 | 72 | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17430 | 112 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17361 | 73 | https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17280 | 97 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17164 | 78 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 17157 | 81 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | /vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 | GET | 200 | 799 | 354 | https://www.tarotnow.xyz/vi/reading/session/502d9f03-743c-491c-9594-96009c9f21c9 |
| logged-in-admin | desktop | /vi/collection | GET | 200 | 794 | 602 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Favatars%2F0e2a5092da564526beb9b942ecdc0e03-bb32559e68264e6c95cffbca86f9a6cc.webp&w=32&q=75 |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 789 | 253 | https://www.tarotnow.xyz/_next/static/chunks/07o~3chaybnsv.js |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 786 | 315 | https://www.tarotnow.xyz/api/auth/session |
| logged-in-admin | mobile | /vi/community | GET | 200 | 783 | 300 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 780 | 110 | https://img.tarotnow.xyz/light-god-50/03_The_High%20Priestess%20_50_20260325_181348.avif |
| logged-in-admin | mobile | /vi/admin | GET | 200 | 774 | 320 | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | /vi/community | GET | 200 | 773 | 323 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | /vi/reading/session/c8026be2-c55b-4692-b014-3d9ecf3a67f0 | GET | 200 | 771 | 298 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | /vi/community | GET | 200 | 767 | 297 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 767 | 363 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 764 | 123 | https://img.tarotnow.xyz/light-god-50/10_The_Hermit_50_20260325_181353.avif |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 760 | 149 | https://img.tarotnow.xyz/light-god-50/09_Strength_50_20260325_181351.avif |
| logged-in-admin | desktop | /vi/admin | GET | 200 | 755 | 284 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | /vi/collection | GET | 200 | 755 | 354 | https://img.tarotnow.xyz/light-god-50/07_The_Lovers_50_20260325_181351.avif |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Route | Count | Request Key |
| --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-0-hlnlst9psev.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/09njxa758vvw_.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/07cbcba783g-t.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/04bi685.wk-4b.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12gvu2nm._6pa.js |
| logged-in-admin | desktop | /vi/collection | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0r68tfefeu~dz.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reload Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 142 | 1 | 140 | 71 | 71 | 0 |
| logged-in-reader | desktop | 142 | 3 | 123 | 71 | 71 | 0 |
| logged-in-admin | mobile | 38 | 16 | 16 | 19 | 19 | 0 |
| logged-in-reader | mobile | 38 | 17 | 20 | 19 | 19 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.