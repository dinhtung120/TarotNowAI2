# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T16:07:29.424Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: targeted-hotspots
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 10 | 32.1 | 3823 | 1 | 0 | 0 | 8 | 0 | yes |
| logged-in-reader | desktop | 9 | 32.0 | 5437 | 15 | 0 | 0 | 14 | 0 | yes |
| logged-out | mobile | 0 | 0.0 | 0 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 10 | 30.3 | 3519 | 0 | 0 | 0 | 3 | 0 | yes |
| logged-in-reader | mobile | 9 | 29.7 | 3713 | 0 | 0 | 0 | 1 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 1 | 29.0 | 3204 | 0 |
| logged-in-admin | desktop | collection | 1 | 33.0 | 6835 | 1 |
| logged-in-admin | desktop | community | 1 | 34.0 | 4656 | 0 |
| logged-in-admin | desktop | readers | 3 | 29.7 | 3484 | 0 |
| logged-in-admin | desktop | reading | 3 | 35.3 | 3368 | 0 |
| logged-in-admin | desktop | wallet | 1 | 30.0 | 2974 | 0 |
| logged-in-admin | mobile | admin | 1 | 29.0 | 2806 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5526 | 0 |
| logged-in-admin | mobile | community | 1 | 34.0 | 4075 | 0 |
| logged-in-admin | mobile | readers | 3 | 29.7 | 3271 | 0 |
| logged-in-admin | mobile | reading | 3 | 29.3 | 3087 | 0 |
| logged-in-admin | mobile | wallet | 1 | 34.0 | 3705 | 0 |
| logged-in-reader | desktop | collection | 1 | 32.0 | 7992 | 4 |
| logged-in-reader | desktop | community | 1 | 37.0 | 5712 | 0 |
| logged-in-reader | desktop | readers | 3 | 29.7 | 5351 | 6 |
| logged-in-reader | desktop | reading | 3 | 33.7 | 4909 | 5 |
| logged-in-reader | desktop | wallet | 1 | 29.0 | 4447 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5518 | 0 |
| logged-in-reader | mobile | community | 1 | 34.0 | 5880 | 0 |
| logged-in-reader | mobile | readers | 3 | 28.7 | 2827 | 0 |
| logged-in-reader | mobile | reading | 3 | 30.0 | 3486 | 0 |
| logged-in-reader | mobile | wallet | 1 | 28.0 | 3079 | 0 |

## Key Findings
1. Critical: 5 page(s) vượt 35 requests.
2. High: 38 page(s) vượt 25 requests.
3. High: 45 request vượt 800ms.
4. Medium: 66 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 7992 | 32 | 5000 | 24.0 | 0.0039 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6835 | 33 | 1052 | 0.0 | 0.0043 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 6378 | 31 | 4396 | 0.0 | 0.0039 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 5880 | 34 | 1668 | 0.0 | 0.0173 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 5794 | 29 | 3836 | 0.0 | 0.0039 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | 5776 | 36 | 3676 | 0.0 | 0.0039 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 5712 | 37 | 4040 | 0.0 | 0.0039 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5526 | 29 | 564 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5518 | 29 | 556 | 0.0 | 0.0000 |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | 4799 | 36 | 2736 | 0.0 | 0.0039 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 4805 | 2232 | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 4301 | 2434 | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 3726 | 1780 | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 3554 | 1374 | https://www.tarotnow.xyz/vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 2803 | 544 | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 2759 | 2742 | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 2588 | 490 | https://www.tarotnow.xyz/vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 2389 | 594 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reading | /vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc | GET | 200 | 2093 | 516 | https://www.tarotnow.xyz/vi/reading/session/a5f8f1c9-6d19-4afc-a247-c624365ff6bc |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 2049 | 517 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 2028 | 768 | https://www.tarotnow.xyz/api/reading/cards-catalog |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1927 | 780 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1819 | 525 | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reading | /vi/reading/session/9f154052-ee11-49da-a0c2-7e7e9619e9b7 | GET | 200 | 1637 | 1225 | https://www.tarotnow.xyz/api/auth/session |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1579 | 1219 | https://www.tarotnow.xyz/api/auth/session |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | reading | /vi/reading/session/6824a31f-8492-49af-bf22-4d1a164b24f8 | GET | 200 | 795 | 788 | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 792 | 775 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 790 | 403 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 786 | 305 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 779 | 303 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 771 | 308 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 770 | 344 | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 765 | 743 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 758 | 375 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reading | /vi/reading/session/18b9cb87-1bbd-4bb9-8b20-bdd137ccb0d9 | GET | 200 | 752 | 131 | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | GET | 200 | 750 | 325 | https://www.tarotnow.xyz/vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 749 | 334 | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 746 | 345 | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 724 | 313 | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | reading | /vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be | GET | 200 | 724 | 312 | https://www.tarotnow.xyz/vi/reading/session/0d01e4f9-623b-47ee-809c-5ec9d8edb6be |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | reading | /vi/reading/session/6edd7197-a24d-4d21-9f84-53dcfc98ff09 | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | reading | /vi/reading/session/41058d9b-941d-4d9b-a905-a9f5073451ec | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | 0 | 0 | 0 | 0 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.