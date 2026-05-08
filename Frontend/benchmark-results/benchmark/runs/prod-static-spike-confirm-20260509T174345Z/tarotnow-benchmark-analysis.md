# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-08T17:54:33.188Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 25.0 | 3186 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 29.9 | 2993 | 2 | 0 | 1 | 8 | 0 | yes |
| logged-in-reader | desktop | 33 | 29.6 | 3032 | 0 | 0 | 1 | 11 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 2911 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.4 | 3777 | 6 | 0 | 0 | 20 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.8 | 3098 | 1 | 0 | 1 | 11 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.2 | 2855 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2847 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6560 | 0 |
| logged-in-admin | desktop | community | 1 | 34.0 | 3774 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 2912 | 0 |
| logged-in-admin | desktop | gamification | 1 | 29.0 | 3182 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2694 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 3112 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 30.0 | 2887 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2751 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2896 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 2908 | 1 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2753 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2797 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.0 | 2804 | 0 |
| logged-in-admin | desktop | wallet | 4 | 40.8 | 3261 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.8 | 3641 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3479 | 0 |
| logged-in-admin | mobile | collection | 1 | 30.0 | 6220 | 0 |
| logged-in-admin | mobile | community | 1 | 38.0 | 4866 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.5 | 3563 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 3310 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 3456 | 0 |
| logged-in-admin | mobile | inventory | 1 | 37.0 | 3554 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 4211 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 3711 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 4940 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 4123 | 3 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 3291 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 3846 | 2 |
| logged-in-admin | mobile | reading | 5 | 29.6 | 3605 | 0 |
| logged-in-admin | mobile | wallet | 4 | 29.3 | 3271 | 1 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 3033 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6906 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3479 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.5 | 2965 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3036 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3142 | 0 |
| logged-in-reader | desktop | inventory | 1 | 31.0 | 2841 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 3194 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2713 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2809 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.3 | 3006 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2706 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2748 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.4 | 2818 | 0 |
| logged-in-reader | desktop | wallet | 4 | 36.0 | 3122 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2788 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5603 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3548 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 2980 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2849 | 1 |
| logged-in-reader | mobile | home | 1 | 29.0 | 3064 | 0 |
| logged-in-reader | mobile | inventory | 1 | 36.0 | 2908 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 31.0 | 2796 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2732 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2784 | 0 |
| logged-in-reader | mobile | profile | 3 | 31.7 | 3141 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2760 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2776 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 2848 | 0 |
| logged-in-reader | mobile | wallet | 4 | 34.5 | 3971 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 3085 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4568 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2895 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2709 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3274 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3128 | 0 |

## Key Findings
1. Critical: 9 page(s) vượt 35 requests.
2. High: 142 page(s) vượt 25 requests.
3. High: 262 request vượt 800ms.
4. Medium: 753 request trong dải 400-800ms.
5. High: phát hiện 3 handshake redirect(s), cần kiểm tra vòng lặp auth/session.
6. Collection-focus: 0 image request(s) >800ms trên 0 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 7451 | 54 | 944 | 0.0 | 0.0055 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6906 | 29 | 996 | 0.0 | 0.0040 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6560 | 29 | 576 | 16.0 | 0.0042 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 6220 | 30 | 1260 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5603 | 29 | 744 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 4940 | 28 | 2544 | 0.0 | 0.0000 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 4866 | 38 | 2168 | 0.0 | 0.0051 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 4850 | 28 | 1948 | 0.0 | 0.0071 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 4650 | 28 | 1500 | 0.0 | 0.0760 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 4578 | 80 | 1192 | 88.0 | 0.0039 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 4121 | 2805 | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 2000 | 1571 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1829 | 1478 | https://www.tarotnow.xyz/vi/notifications |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1695 | 879 | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1650 | 946 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1546 | 763 | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1541 | 602 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1518 | 525 | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1406 | 644 | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 1348 | 725 | https://www.tarotnow.xyz/vi/gacha/history |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 1333 | 288 | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1315 | 298 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1308 | 417 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 1292 | 610 | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1264 | 1245 | https://www.tarotnow.xyz/vi/readers |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 800 | 442 | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 800 | 228 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 799 | 319 | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 798 | 226 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 798 | 236 | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 797 | 269 | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 797 | 455 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 796 | 323 | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 796 | 225 | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 796 | 792 | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 796 | 235 | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 795 | 334 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 782 | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 795 | 270 | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 793 | 317 | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 3 | GET https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |

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