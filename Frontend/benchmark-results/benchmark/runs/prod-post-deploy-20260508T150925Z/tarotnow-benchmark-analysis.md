# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T15:28:23.010Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 5285 | 1 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 38 | 30.1 | 3463 | 3 | 0 | 0 | 19 | 0 | yes |
| logged-in-reader | desktop | 38 | 30.4 | 3349 | 3 | 0 | 0 | 13 | 0 | yes |
| logged-out | mobile | 9 | 25.2 | 8953 | 3 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 35 | 33.6 | 11089 | 130 | 0 | 0 | 39 | 0 | yes |
| logged-in-reader | mobile | 35 | 28.4 | 6555 | 91 | 0 | 0 | 3 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | auth | 5 | 29.8 | 3298 | 1 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2816 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6611 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4289 | 0 |
| logged-in-admin | desktop | gacha | 2 | 37.5 | 4418 | 0 |
| logged-in-admin | desktop | gamification | 1 | 32.0 | 3364 | 0 |
| logged-in-admin | desktop | home | 1 | 39.0 | 4975 | 0 |
| logged-in-admin | desktop | inventory | 1 | 36.0 | 3110 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 33.0 | 3294 | 0 |
| logged-in-admin | desktop | legal | 3 | 26.3 | 3337 | 0 |
| logged-in-admin | desktop | notifications | 1 | 33.0 | 4081 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 3220 | 2 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 4174 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 3173 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.6 | 2857 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 3341 | 0 |
| logged-in-admin | mobile | auth | 5 | 43.2 | 12368 | 18 |
| logged-in-admin | mobile | chat | 1 | 31.0 | 6600 | 3 |
| logged-in-admin | mobile | collection | 1 | 32.0 | 4274 | 14 |
| logged-in-admin | mobile | community | 1 | 37.0 | 11624 | 4 |
| logged-in-admin | mobile | gacha | 2 | 33.0 | 4660 | 7 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 15128 | 3 |
| logged-in-admin | mobile | home | 1 | 35.0 | 6081 | 6 |
| logged-in-admin | mobile | inventory | 1 | 35.0 | 3867 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 6209 | 2 |
| logged-in-admin | mobile | legal | 3 | 25.7 | 10651 | 6 |
| logged-in-admin | mobile | notifications | 1 | 29.0 | 17258 | 3 |
| logged-in-admin | mobile | profile | 3 | 42.0 | 19043 | 3 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 15373 | 3 |
| logged-in-admin | mobile | readers | 4 | 30.5 | 9234 | 10 |
| logged-in-admin | mobile | reading | 5 | 32.0 | 9846 | 14 |
| logged-in-admin | mobile | wallet | 4 | 29.8 | 13822 | 34 |
| logged-in-reader | desktop | auth | 5 | 35.6 | 4210 | 3 |
| logged-in-reader | desktop | chat | 1 | 31.0 | 3214 | 0 |
| logged-in-reader | desktop | collection | 1 | 30.0 | 6120 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3592 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.5 | 3531 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3099 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2856 | 0 |
| logged-in-reader | desktop | inventory | 1 | 41.0 | 4616 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2888 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.7 | 2859 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2977 | 0 |
| logged-in-reader | desktop | profile | 3 | 31.3 | 3094 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 3488 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2969 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 2884 | 0 |
| logged-in-reader | desktop | wallet | 4 | 29.5 | 3305 | 0 |
| logged-in-reader | mobile | auth | 5 | 24.0 | 5337 | 53 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 6238 | 1 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 7864 | 3 |
| logged-in-reader | mobile | community | 1 | 34.0 | 9830 | 1 |
| logged-in-reader | mobile | gacha | 2 | 33.0 | 5944 | 4 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 5695 | 0 |
| logged-in-reader | mobile | home | 1 | 35.0 | 5856 | 6 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 5525 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 6922 | 2 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 7557 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 7617 | 1 |
| logged-in-reader | mobile | profile | 3 | 28.3 | 5990 | 5 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 6158 | 2 |
| logged-in-reader | mobile | readers | 4 | 28.0 | 6914 | 3 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 6010 | 6 |
| logged-in-reader | mobile | wallet | 4 | 28.0 | 7695 | 4 |
| logged-out | desktop | auth | 5 | 24.0 | 5566 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4085 | 1 |
| logged-out | desktop | legal | 3 | 25.0 | 5218 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 8354 | 1 |
| logged-out | mobile | home | 1 | 31.0 | 3264 | 2 |
| logged-out | mobile | legal | 3 | 25.0 | 11847 | 0 |

## Key Findings
1. Critical: 15 page(s) vượt 35 requests.
2. High: 126 page(s) vượt 25 requests.
3. High: 1158 request vượt 800ms.
4. Medium: 652 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 32003 | 64 | 11240 | 0.0 | 0.0760 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 22654 | 31 | - | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 17258 | 29 | 15244 | 0.0 | 0.0074 |
| logged-in-admin | mobile | auth-public | /vi/login | 17120 | 58 | 7764 | 0.0 | 0.0024 |
| logged-in-admin | mobile | reading | /vi/reading/history | 16173 | 31 | 14188 | 0.0 | 0.0071 |
| logged-in-admin | mobile | auth-public | /vi/register | 15667 | 58 | 6700 | 0.0 | 0.0024 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 15501 | 25 | 11592 | 0.0 | 0.0000 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 15373 | 31 | 13028 | 0.0 | 0.0071 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 15128 | 29 | 12644 | 0.0 | 0.0071 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 13918 | 31 | 5340 | 0.0 | 0.0760 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 15193 | 12886 | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 14096 | 10688 | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 13315 | 11257 | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 12798 | 10445 | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 12585 | 10723 | https://www.tarotnow.xyz/vi/gamification |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 12276 | 11116 | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 11315 | 9681 | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 11192 | 9579 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 10239 | 9800 | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-out | mobile | auth-public | /vi/verify-email | GET | 200 | 9943 | 8811 | https://www.tarotnow.xyz/vi/verify-email |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 9209 | 6316 | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 9126 | 7364 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 8866 | 7356 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 8842 | 7540 | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 8737 | 6366 | https://www.tarotnow.xyz/vi/community |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | auth-public | /vi/verify-email | GET | 200 | 800 | 404 | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | auth-public | /vi/login | GET | 200 | 799 | 587 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | auth-public | /vi/forgot-password | GET | 200 | 798 | 632 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 797 | 321 | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 796 | 152 | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 796 | 352 | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 796 | 792 | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 794 | 224 | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 794 | 449 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 793 | 342 | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | auth-public | /vi/reset-password | GET | 200 | 793 | 394 | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 792 | 117 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 791 | 117 | https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 790 | 341 | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | auth-public | /vi/verify-email | GET | 200 | 789 | 323 | https://www.tarotnow.xyz/vi/verify-email |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 3 | GET https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | desktop | auth-public | /vi | 2 | POST https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0e5kgac227qbt.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | auth-public | /vi/forgot-password | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0b8cs8wbqiq6e.js |

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