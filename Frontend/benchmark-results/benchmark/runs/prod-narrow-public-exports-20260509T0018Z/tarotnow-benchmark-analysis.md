# TarotNow Benchmark Analysis

- Run time (UTC): 2026-05-09T00:26:06.346Z
- Base: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader

## Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 24.9 | 2886 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 44 | 28.9 | 2953 | 0 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | desktop | 34 | 28.5 | 2926 | 0 | 0 | 0 | 7 | 0 | yes |
| logged-out | mobile | 9 | 25.1 | 2857 | 0 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 29.7 | 3058 | 0 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | mobile | 33 | 29.1 | 2960 | 0 | 0 | 0 | 18 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 2927 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 3305 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6337 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 3999 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2861 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3291 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2695 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2869 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3204 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2655 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2726 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2888 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2728 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2759 | 0 |
| logged-in-admin | desktop | reading | 6 | 29.2 | 2772 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2808 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.4 | 2848 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3898 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5771 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3935 | 0 |
| logged-in-admin | mobile | gacha | 2 | 30.0 | 2912 | 0 |
| logged-in-admin | mobile | gamification | 1 | 32.0 | 3474 | 0 |
| logged-in-admin | mobile | home | 1 | 34.0 | 2941 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2925 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3246 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2762 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2838 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2877 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2759 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2886 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 2836 | 0 |
| logged-in-admin | mobile | wallet | 4 | 36.0 | 3531 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2758 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6230 | 0 |
| logged-in-reader | desktop | community | 1 | 35.0 | 3620 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.5 | 2867 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2847 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2765 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2836 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2802 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2711 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2843 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2890 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2723 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2792 | 0 |
| logged-in-reader | desktop | reading | 6 | 29.2 | 2801 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 2793 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2754 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5585 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3657 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 2917 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2881 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2878 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2891 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2849 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2735 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2888 | 0 |
| logged-in-reader | mobile | profile | 3 | 28.7 | 2890 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2816 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.4 | 2838 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.4 | 2875 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.8 | 2880 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2755 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4065 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2710 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2734 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3529 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2837 | 0 |

## Key Findings
1. Critical: 2 page(s) vượt 35 requests.
2. High: 144 page(s) vượt 25 requests.
3. High: 45 request vượt 800ms.
4. Medium: 282 request trong dải 400-800ms.
5. Không phát hiện handshake redirect bất thường.
6. Collection-focus: 6 image request(s) >800ms trên 39 image request(s).

## Top Slow Pages
| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 6337 | 29 | 540 | 0.0 | 0.0042 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 6230 | 29 | 496 | 0.0 | 0.0040 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 5771 | 29 | 820 | 0.0 | 0.0000 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 5585 | 29 | 456 | 0.0 | 0.0000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 4718 | 59 | 1388 | 0.0 | 0.0071 |
| logged-out | desktop | auth-public | /vi | 4065 | 29 | 1516 | 522.0 | 0.0000 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 3999 | 35 | 2112 | 0.0 | 0.0041 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 3935 | 36 | 2156 | 0.0 | 0.0051 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 3898 | 28 | 920 | 0.0 | 0.0071 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 3657 | 35 | 1608 | 0.0 | 0.0051 |

## High Slow Requests (> 800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1303 | 187 | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1270 | 587 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1239 | 777 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 1222 | 330 | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1221 | 366 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1159 | 189 | https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1111 | 593 | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1065 | 389 | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1056 | 571 | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1041 | 185 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1017 | 635 | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 991 | 585 | https://www.tarotnow.xyz/_next/static/chunks/0eqe0oax2~t9t.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 941 | 379 | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 928 | 380 | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 894 | 573 | https://www.tarotnow.xyz/vi/login |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 797 | 322 | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 794 | 451 | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 793 | 329 | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 790 | 327 | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 789 | 543 | https://www.tarotnow.xyz/_next/static/chunks/0xl.ndi-x969l.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 788 | 303 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 788 | 292 | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 788 | 310 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 786 | 351 | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 785 | 316 | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | mobile | reading | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | GET | 200 | 785 | 334 | https://www.tarotnow.xyz/vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 784 | 322 | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 782 | 311 | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 781 | 352 | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 779 | 330 | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |

## Duplicate Request Candidates (Non-telemetry)
| Scenario | Viewport | Feature | Route | Count | Request Key |
| --- | --- | --- | --- | ---: | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 2 | GET https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |

## Collection Image Metrics
| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 13 | 4 | 3 | 12 | 1 | 0 |
| logged-in-reader | desktop | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | 14 | 4 | 3 | 13 | 1 | 0 |
| logged-in-reader | mobile | 6 | 2 | 0 | 6 | 0 | 0 |

## Notes
- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.
- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.