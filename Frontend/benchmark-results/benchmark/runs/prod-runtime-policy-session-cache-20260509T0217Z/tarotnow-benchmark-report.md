# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T19:27:48.928Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 3
- High pages (request count): 142
- High slow requests: 27
- Medium slow requests: 190

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2855 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2968 | 1260 | 0 | 0 | 16 | 0 | yes |
| logged-in-reader | desktop | 33 | 2930 | 971 | 0 | 0 | 18 | 0 | yes |
| logged-out | mobile | 9 | 2750 | 222 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2860 | 1244 | 0 | 0 | 11 | 0 | yes |
| logged-in-reader | mobile | 33 | 2926 | 951 | 0 | 0 | 10 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2876 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2867 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6101 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4040 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2910 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3323 | 0 |
| logged-in-admin | desktop | home | 1 | 29.0 | 2883 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 2853 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 32.0 | 3259 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2694 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2768 | 0 |
| logged-in-admin | desktop | profile | 3 | 30.0 | 2910 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2803 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2808 | 0 |
| logged-in-admin | desktop | reading | 5 | 31.0 | 2880 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.3 | 2815 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 2798 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2772 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5492 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3412 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.5 | 2850 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 2948 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2694 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2859 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 2843 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2682 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2654 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 2928 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2638 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2738 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.8 | 2795 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2753 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2759 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6091 | 0 |
| logged-in-reader | desktop | community | 1 | 36.0 | 3703 | 0 |
| logged-in-reader | desktop | gacha | 2 | 32.0 | 2884 | 0 |
| logged-in-reader | desktop | gamification | 1 | 33.0 | 2891 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2705 | 0 |
| logged-in-reader | desktop | inventory | 1 | 33.0 | 2902 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 2847 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2692 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2712 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 2888 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2666 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2774 | 0 |
| logged-in-reader | desktop | reading | 5 | 31.0 | 2857 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.8 | 2805 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2732 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 5802 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3715 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.0 | 2879 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2883 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2725 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2935 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2852 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2680 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2850 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2967 | 0 |
| logged-in-reader | mobile | reader | 1 | 30.0 | 2905 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.3 | 2740 | 0 |
| logged-in-reader | mobile | reading | 5 | 28.8 | 2781 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.5 | 2846 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 2736 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3926 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2696 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2679 | 0 |
| logged-out | mobile | home | 1 | 26.0 | 3196 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2721 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3926 | 1566 | 1915 | 1356 | 1356 | 0.0000 | 245.0 | 601668 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 713 | 713 | 500 | 500 | 0.0000 | 0.0 | 512558 |
| logged-out | desktop | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2675 | 665 | 665 | 480 | 480 | 0.0000 | 0.0 | 513770 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 761 | 761 | 632 | 632 | 0.0000 | 0.0 | 512298 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2796 | 792 | 792 | 524 | 524 | 0.0000 | 0.0 | 512380 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 714 | 714 | 484 | 484 | 0.0000 | 0.0 | 512438 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2651 | 643 | 643 | 452 | 768 | 0.0000 | 0.0 | 526176 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 724 | 724 | 488 | 812 | 0.0000 | 0.0 | 526256 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 693 | 693 | 480 | 480 | 0.0000 | 0.0 | 526357 |
| logged-in-admin | desktop | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 875 | 875 | 924 | 924 | 0.0035 | 98.0 | 608378 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 904 | 925 | 612 | 984 | 0.0041 | 0.0 | 643199 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2853 | 843 | 843 | 524 | 1164 | 0.0041 | 0.0 | 648226 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | 11 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2961 | 954 | 954 | 728 | 1152 | 0.0041 | 0.0 | 726567 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 848 | 848 | 532 | 1128 | 0.0041 | 0.0 | 727561 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 12 | 3 | 7 | 11 | 1 | 0 | 6101 | 771 | 825 | 496 | 880 | 0.0042 | 0.0 | 643733 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3036 | 874 | 1029 | 624 | 1008 | 0.0489 | 0.0 | 637705 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 774 | 783 | 552 | 936 | 0.0041 | 0.0 | 632103 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 30 | 37 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 897 | 986 | 556 | 1216 | 0.0489 | 0.0 | 633571 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2895 | 887 | 887 | 572 | 1164 | 0.0041 | 0.0 | 636695 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 840 | 849 | 560 | 960 | 0.0041 | 0.0 | 632094 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3259 | 870 | 1252 | 616 | 976 | 0.0041 | 0.0 | 654172 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | 4 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4040 | 793 | 1283 | 524 | 1864 | 0.0041 | 0.0 | 779556 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3323 | 864 | 1316 | 640 | 1036 | 0.0279 | 0.0 | 645628 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2897 | 840 | 840 | 480 | 1232 | 0.0041 | 0.0 | 636223 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2810 | 798 | 803 | 568 | 964 | 0.0041 | 0.0 | 632152 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2802 | 791 | 794 | 704 | 1228 | 0.0041 | 0.0 | 632857 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 858 | 858 | 540 | 996 | 0.0041 | 0.0 | 631486 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 749 | 761 | 516 | 920 | 0.0041 | 0.0 | 632317 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2803 | 786 | 798 | 692 | 1056 | 0.0041 | 0.0 | 632815 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 742 | 749 | 592 | 1108 | 0.0041 | 0.0 | 633110 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 662 | 662 | 480 | 820 | 0.0020 | 0.0 | 526525 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 671 | 671 | 500 | 788 | 0.0020 | 0.0 | 526548 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 726 | 727 | 532 | 844 | 0.0020 | 0.0 | 526616 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3205 | 840 | 1197 | 704 | 1084 | 0.0000 | 0.0 | 648009 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 822 | 843 | 604 | 1048 | 0.0000 | 0.0 | 648083 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 728 | 740 | 512 | 848 | 0.0000 | 0.0 | 646369 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3203 | 708 | 1196 | 532 | 860 | 0.0022 | 0.0 | 699885 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 733 | 739 | 628 | 988 | 0.0000 | 0.0 | 645026 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 747 | 754 | 684 | 1008 | 0.0000 | 0.0 | 646849 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 802 | 808 | 584 | 1080 | 0.0000 | 0.0 | 649139 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 751 | 752 | 528 | 956 | 0.0000 | 0.0 | 656417 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 854 | 856 | 768 | 1156 | 0.0000 | 0.0 | 650245 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2798 | 779 | 789 | 556 | 868 | 0.0000 | 0.0 | 646275 |
| logged-in-admin | desktop | reading | /vi/reading/session/5fa7eae7-70e6-49b0-810e-0e4374cbc30a | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2934 | 748 | 925 | 512 | 876 | 0.0041 | 0.0 | 727246 |
| logged-in-admin | desktop | reading | /vi/reading/session/f9a61880-f47e-4668-a0a0-8c52736e8b39 | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2989 | 826 | 979 | 524 | 916 | 0.0041 | 0.0 | 727105 |
| logged-in-admin | desktop | reading | /vi/reading/session/1f9d7a0c-07ee-4ed7-be32-07168cff65d2 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 765 | 783 | 576 | 960 | 0.0041 | 0.0 | 632582 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 761 | 764 | 520 | 884 | 0.0041 | 0.0 | 631502 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 782 | 817 | 520 | 908 | 0.0041 | 0.0 | 631424 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 799 | 804 | 600 | 980 | 0.0041 | 0.0 | 631544 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 677 | 684 | 508 | 988 | 0.0041 | 0.0 | 633401 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 825 | 832 | 748 | 1292 | 0.0041 | 0.0 | 633462 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 781 | 813 | 496 | 1028 | 0.0041 | 0.0 | 633511 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2705 | 691 | 692 | 556 | 1092 | 0.0033 | 214.0 | 538102 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2949 | 924 | 932 | 544 | 1016 | 0.0039 | 0.0 | 643113 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 2 | 0 | 5 | 0 | 0 | 2902 | 892 | 892 | 512 | 1140 | 0.0039 | 0.0 | 648484 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2860 | 849 | 849 | 476 | 1132 | 0.0039 | 0.0 | 728704 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 896 | 896 | 560 | 1108 | 0.0039 | 0.0 | 725643 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 6 | 3 | 0 | 6 | 0 | 0 | 6091 | 679 | 680 | 500 | 500 | 0.0040 | 0.0 | 642856 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3015 | 779 | 1007 | 496 | 876 | 0.0726 | 0.0 | 638293 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 796 | 805 | 516 | 892 | 0.0039 | 0.0 | 632021 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 30 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 809 | 810 | 508 | 1260 | 0.0000 | 0.0 | 635161 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 839 | 840 | 460 | 1084 | 0.0039 | 0.0 | 636784 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 751 | 754 | 500 | 892 | 0.0039 | 0.0 | 632306 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 837 | 838 | 484 | 1140 | 0.0039 | 0.0 | 654878 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3703 | 850 | 970 | 528 | 1760 | 0.0039 | 0.0 | 779525 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2891 | 881 | 882 | 500 | 924 | 0.0277 | 0.0 | 647812 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 885 | 892 | 592 | 1096 | 0.0039 | 0.0 | 634717 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2663 | 655 | 655 | 484 | 884 | 0.0039 | 0.0 | 632043 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2779 | 767 | 770 | 492 | 908 | 0.0039 | 0.0 | 632328 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 31 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 863 | 863 | 464 | 1156 | 0.0095 | 0.0 | 636598 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2712 | 701 | 701 | 480 | 892 | 0.0040 | 0.0 | 632838 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2666 | 656 | 656 | 480 | 852 | 0.0039 | 0.0 | 632736 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 770 | 770 | 684 | 1224 | 0.0039 | 0.0 | 633509 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 692 | 693 | 484 | 816 | 0.0019 | 0.0 | 526595 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2644 | 635 | 635 | 484 | 792 | 0.0019 | 0.0 | 526560 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2728 | 715 | 715 | 476 | 792 | 0.0019 | 0.0 | 526610 |
| logged-in-reader | desktop | reading | /vi/reading/session/08e68137-7b1a-4d6d-93e0-f0fb46b48a64 | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 806 | 894 | 480 | 892 | 0.0039 | 0.0 | 727165 |
| logged-in-reader | desktop | reading | /vi/reading/session/381295fa-cd58-4d88-9d10-ed0e9785864a | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 705 | 705 | 504 | 876 | 0.0039 | 0.0 | 632665 |
| logged-in-reader | desktop | reading | /vi/reading/session/01933324-a217-44aa-9f06-9d22197a9c30 | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2933 | 797 | 921 | 508 | 868 | 0.0039 | 0.0 | 727157 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 783 | 791 | 484 | 876 | 0.0039 | 0.0 | 631772 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 728 | 729 | 520 | 900 | 0.0039 | 0.0 | 631918 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 770 | 779 | 500 | 860 | 0.0039 | 0.0 | 631700 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 705 | 705 | 488 | 996 | 0.0039 | 0.0 | 633372 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 806 | 806 | 516 | 1048 | 0.0039 | 0.0 | 633465 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 669 | 669 | 480 | 948 | 0.0039 | 0.0 | 633795 |
| logged-out | mobile | auth-public | /vi | 26 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3196 | 1168 | 1188 | 1028 | 1384 | 0.0000 | 0.0 | 531260 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2665 | 636 | 654 | 548 | 548 | 0.0000 | 0.0 | 512679 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 704 | 704 | 500 | 500 | 0.0000 | 0.0 | 513818 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2679 | 667 | 667 | 456 | 456 | 0.0000 | 0.0 | 512216 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2675 | 669 | 669 | 468 | 468 | 0.0000 | 0.0 | 512507 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2656 | 645 | 645 | 448 | 448 | 0.0000 | 0.0 | 512394 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 711 | 712 | 464 | 784 | 0.0000 | 0.0 | 526163 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2680 | 667 | 667 | 472 | 472 | 0.0000 | 0.0 | 526173 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 745 | 745 | 512 | 824 | 0.0000 | 0.0 | 526203 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2694 | 685 | 685 | 436 | 788 | 0.0032 | 0.0 | 537876 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 809 | 836 | 416 | 808 | 0.0000 | 0.0 | 645406 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2859 | 847 | 847 | 656 | 996 | 0.0000 | 0.0 | 645111 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 1 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2835 | 826 | 826 | 444 | 1076 | 0.0000 | 0.0 | 727628 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 848 | 849 | 436 | 832 | 0.0000 | 0.0 | 729675 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 3 | 4 | 12 | 1 | 0 | 5492 | 748 | 779 | 452 | 768 | 0.0000 | 0.0 | 643612 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3093 | 856 | 1086 | 464 | 1116 | 0.0689 | 0.0 | 637108 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 806 | 809 | 548 | 880 | 0.0000 | 0.0 | 632002 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 860 | 949 | 480 | 1124 | 0.0760 | 0.0 | 631223 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 818 | 818 | 428 | 1040 | 0.0000 | 0.0 | 636165 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 755 | 762 | 436 | 756 | 0.0000 | 0.0 | 631858 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 830 | 830 | 452 | 1092 | 0.0000 | 0.0 | 653601 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3412 | 647 | 661 | 416 | 1820 | 0.0051 | 0.0 | 643505 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2948 | 889 | 931 | 516 | 848 | 0.0000 | 0.0 | 644240 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2868 | 856 | 857 | 464 | 1140 | 0.0000 | 0.0 | 636640 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 693 | 709 | 440 | 776 | 0.0000 | 0.0 | 631873 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 717 | 727 | 516 | 920 | 0.0000 | 0.0 | 632726 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 843 | 843 | 452 | 1092 | 0.0071 | 0.0 | 631464 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2654 | 625 | 640 | 408 | 748 | 0.0000 | 0.0 | 632547 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2638 | 612 | 625 | 420 | 756 | 0.0000 | 0.0 | 632980 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 684 | 696 | 424 | 768 | 0.0000 | 0.0 | 633462 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2638 | 627 | 627 | 424 | 732 | 0.0032 | 0.0 | 526560 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2657 | 648 | 648 | 432 | 740 | 0.0032 | 0.0 | 526453 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 738 | 739 | 452 | 764 | 0.0032 | 0.0 | 526710 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 657 | 673 | 436 | 776 | 0.0000 | 0.0 | 647892 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2661 | 637 | 651 | 428 | 752 | 0.0000 | 0.0 | 647953 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2817 | 792 | 802 | 528 | 848 | 0.0000 | 0.0 | 646288 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 800 | 893 | 448 | 800 | 0.0000 | 0.0 | 698783 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 725 | 733 | 468 | 776 | 0.0000 | 0.0 | 644997 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 704 | 718 | 448 | 760 | 0.0000 | 0.0 | 646836 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2817 | 790 | 806 | 464 | 808 | 0.0000 | 0.0 | 649216 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2913 | 785 | 891 | 468 | 856 | 0.0000 | 0.0 | 689578 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2860 | 774 | 845 | 460 | 1100 | 0.0000 | 0.0 | 651847 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 816 | 834 | 568 | 876 | 0.0000 | 0.0 | 646547 |
| logged-in-admin | mobile | reading | /vi/reading/session/6667d52e-12f3-44bf-98f3-d3fd8dcf9b30 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2901 | 741 | 893 | 448 | 792 | 0.0000 | 0.0 | 695172 |
| logged-in-admin | mobile | reading | /vi/reading/session/071cc73e-5c79-4ad6-b58b-688a5f10e9d0 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 821 | 833 | 668 | 996 | 0.0000 | 0.0 | 632864 |
| logged-in-admin | mobile | reading | /vi/reading/session/c01ce89d-3405-4c5b-ba53-2422722b518a | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2663 | 629 | 646 | 440 | 768 | 0.0000 | 0.0 | 632744 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 707 | 718 | 456 | 796 | 0.0000 | 0.0 | 631640 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2700 | 673 | 690 | 432 | 764 | 0.0000 | 0.0 | 631626 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2709 | 677 | 693 | 464 | 796 | 0.0000 | 0.0 | 631695 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2741 | 714 | 728 | 480 | 816 | 0.0000 | 0.0 | 633104 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 687 | 696 | 436 | 772 | 0.0000 | 0.0 | 633472 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 728 | 737 | 452 | 792 | 0.0000 | 0.0 | 633564 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 713 | 713 | 464 | 816 | 0.0032 | 0.0 | 538009 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 841 | 866 | 480 | 852 | 0.0000 | 0.0 | 644331 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2935 | 922 | 922 | 800 | 1140 | 0.0000 | 0.0 | 645082 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2871 | 857 | 857 | 476 | 1132 | 0.0000 | 0.0 | 725363 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 875 | 875 | 480 | 1116 | 0.0000 | 0.0 | 726849 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | 22 | high | 0 | 0 | 1 | 0 | 5 | 4 | 0 | 5 | 0 | 0 | 5802 | 957 | 1007 | 512 | 972 | 0.0071 | 4.0 | 644768 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3122 | 848 | 1110 | 476 | 1108 | 0.0821 | 0.0 | 638191 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2939 | 907 | 925 | 812 | 1148 | 0.0000 | 0.0 | 631837 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 820 | 829 | 492 | 820 | 0.0000 | 0.0 | 632699 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 875 | 875 | 480 | 1100 | 0.0000 | 0.0 | 636494 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 693 | 720 | 460 | 784 | 0.0000 | 0.0 | 632199 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 833 | 841 | 468 | 1128 | 0.0000 | 0.0 | 651587 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3715 | 802 | 978 | 524 | 1688 | 0.0051 | 0.0 | 779155 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2883 | 865 | 865 | 536 | 876 | 0.0000 | 0.0 | 643152 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 866 | 866 | 472 | 1128 | 0.0000 | 0.0 | 636885 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 754 | 776 | 600 | 944 | 0.0000 | 0.0 | 631883 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2839 | 810 | 827 | 560 | 884 | 0.0000 | 0.0 | 632054 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 858 | 858 | 728 | 1084 | 0.0330 | 0.0 | 633273 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 834 | 834 | 456 | 784 | 0.0000 | 0.0 | 634841 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 865 | 890 | 528 | 880 | 0.0000 | 0.0 | 635235 |
| logged-in-reader | mobile | reading | /vi/reading/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2837 | 746 | 821 | 460 | 888 | 0.0000 | 0.0 | 635435 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2673 | 660 | 660 | 468 | 780 | 0.0032 | 0.0 | 526514 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2690 | 677 | 677 | 476 | 784 | 0.0032 | 0.0 | 526337 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2676 | 665 | 665 | 456 | 772 | 0.0032 | 0.0 | 526661 |
| logged-in-reader | mobile | reading | /vi/reading/session/af0de376-971e-4cc9-a333-3f30db229b2d | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 678 | 704 | 452 | 804 | 0.0000 | 0.0 | 632800 |
| logged-in-reader | mobile | reading | /vi/reading/session/76caae04-afc0-4374-a93a-951155efd887 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2729 | 690 | 718 | 456 | 788 | 0.0000 | 0.0 | 632693 |
| logged-in-reader | mobile | reading | /vi/reading/session/867ca370-3c6e-4306-afdb-ab9f25cbc558 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 699 | 726 | 452 | 784 | 0.0000 | 0.0 | 632716 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 648 | 677 | 444 | 780 | 0.0000 | 0.0 | 631672 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2696 | 658 | 682 | 448 | 780 | 0.0000 | 0.0 | 631740 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 748 | 771 | 476 | 800 | 0.0000 | 0.0 | 632117 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2678 | 640 | 666 | 436 | 824 | 0.0000 | 0.0 | 633448 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 702 | 730 | 492 | 880 | 0.0000 | 0.0 | 633439 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2695 | 656 | 683 | 448 | 792 | 0.0000 | 0.0 | 633609 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 12 | 3 | 7 | 11 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 3 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 3 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 5 | 4 | 0 | 5 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/5fa7eae7-70e6-49b0-810e-0e4374cbc30a | 35 | high | 3 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/f9a61880-f47e-4668-a0a0-8c52736e8b39 | 35 | high | 3 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/1f9d7a0c-07ee-4ed7-be32-07168cff65d2 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/08e68137-7b1a-4d6d-93e0-f0fb46b48a64 | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/381295fa-cd58-4d88-9d10-ed0e9785864a | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/01933324-a217-44aa-9f06-9d22197a9c30 | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 0 | 29 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/6667d52e-12f3-44bf-98f3-d3fd8dcf9b30 | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/071cc73e-5c79-4ad6-b58b-688a5f10e9d0 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/c01ce89d-3405-4c5b-ba53-2422722b518a | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/af0de376-971e-4cc9-a333-3f30db229b2d | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/76caae04-afc0-4374-a93a-951155efd887 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/867ca370-3c6e-4306-afdb-ab9f25cbc558 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1197 | 597 | html | https://www.tarotnow.xyz/vi |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1071 | 707 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1032 | 535 | static | https://www.tarotnow.xyz/_next/static/chunks/0~vbsxkq2i_qa.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 975 | 511 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 919 | 323 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 877 | 463 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 875 | 388 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 840 | 360 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 834 | 425 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 832 | 341 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 828 | 338 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 827 | 400 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 827 | 339 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 815 | 315 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 815 | 418 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 814 | 356 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 813 | 400 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 811 | 559 | static | https://www.tarotnow.xyz/_next/static/chunks/0jkg7xwzmtv2t.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 811 | 314 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 811 | 328 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 808 | 307 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 808 | 337 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 806 | 336 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 806 | 367 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 805 | 338 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 803 | 376 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 802 | 371 | html | https://www.tarotnow.xyz/vi/gacha/history |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 800 | 322 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 799 | 412 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-out | desktop | auth-public | /vi | GET | 200 | 796 | 556 | static | https://www.tarotnow.xyz/_next/static/chunks/04qofk648hs17.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 796 | 334 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 796 | 313 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 795 | 316 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 792 | 318 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 791 | 310 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 790 | 545 | static | https://www.tarotnow.xyz/_next/static/chunks/0aa.eah0s47oq.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 790 | 332 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 789 | 339 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 789 | 323 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 788 | 308 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 783 | 329 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 783 | 314 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 783 | 302 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 782 | 327 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 782 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 782 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 780 | 319 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 779 | 319 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | reading | /vi/reading/session/071cc73e-5c79-4ad6-b58b-688a5f10e9d0 | GET | 200 | 779 | 326 | html | https://www.tarotnow.xyz/vi/reading/session/071cc73e-5c79-4ad6-b58b-688a5f10e9d0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 779 | 317 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 778 | 349 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 773 | 308 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 772 | 310 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 771 | 305 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 769 | 435 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 766 | 337 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 766 | 303 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 758 | 323 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 757 | 342 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 754 | 418 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 746 | 350 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 742 | 359 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 742 | 322 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 742 | 342 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 742 | 377 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 741 | 402 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 739 | 326 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 738 | 393 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | desktop | reading | /vi/reading/session/f9a61880-f47e-4668-a0a0-8c52736e8b39 | GET | 200 | 736 | 333 | html | https://www.tarotnow.xyz/vi/reading/session/f9a61880-f47e-4668-a0a0-8c52736e8b39 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 736 | 421 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 730 | 325 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 727 | 434 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 725 | 351 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-reader | desktop | reading | /vi/reading/session/08e68137-7b1a-4d6d-93e0-f0fb46b48a64 | GET | 200 | 725 | 309 | html | https://www.tarotnow.xyz/vi/reading/session/08e68137-7b1a-4d6d-93e0-f0fb46b48a64 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 723 | 362 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 720 | 309 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 717 | 324 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 717 | 371 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 715 | 309 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 712 | 424 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 711 | 324 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 710 | 319 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 708 | 398 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | desktop | reading | /vi/reading/session/1f9d7a0c-07ee-4ed7-be32-07168cff65d2 | GET | 200 | 707 | 318 | html | https://www.tarotnow.xyz/vi/reading/session/1f9d7a0c-07ee-4ed7-be32-07168cff65d2 |
| logged-out | desktop | auth-public | /vi | GET | 200 | 706 | 551 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 706 | 348 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 705 | 327 | html | https://www.tarotnow.xyz/vi/collection |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 702 | 326 | html | https://www.tarotnow.xyz/vi/forgot-password |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 702 | 331 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 700 | 339 | html | https://www.tarotnow.xyz/vi/community |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 700 | 385 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 698 | 327 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 698 | 325 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 697 | 314 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 693 | 327 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 692 | 311 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 691 | 326 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 689 | 310 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | reading | /vi/reading/session/6667d52e-12f3-44bf-98f3-d3fd8dcf9b30 | GET | 200 | 689 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/6667d52e-12f3-44bf-98f3-d3fd8dcf9b30 |
| logged-in-admin | desktop | reading | /vi/reading/session/5fa7eae7-70e6-49b0-810e-0e4374cbc30a | GET | 200 | 688 | 336 | html | https://www.tarotnow.xyz/vi/reading/session/5fa7eae7-70e6-49b0-810e-0e4374cbc30a |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 688 | 313 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 687 | 332 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 686 | 303 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 685 | 302 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reading | /vi/reading/session/01933324-a217-44aa-9f06-9d22197a9c30 | GET | 200 | 683 | 301 | html | https://www.tarotnow.xyz/vi/reading/session/01933324-a217-44aa-9f06-9d22197a9c30 |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 682 | 317 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 682 | 305 | html | https://www.tarotnow.xyz/vi/chat |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created 5fa7eae7-70e6-49b0-810e-0e4374cbc30a. |
| logged-in-admin | desktop | reading.init.spread_5: created f9a61880-f47e-4668-a0a0-8c52736e8b39. |
| logged-in-admin | desktop | reading.init.spread_10: created 1f9d7a0c-07ee-4ed7-be32-07168cff65d2. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created 08e68137-7b1a-4d6d-93e0-f0fb46b48a64. |
| logged-in-reader | desktop | reading.init.spread_5: created 381295fa-cd58-4d88-9d10-ed0e9785864a. |
| logged-in-reader | desktop | reading.init.spread_10: created 01933324-a217-44aa-9f06-9d22197a9c30. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 6667d52e-12f3-44bf-98f3-d3fd8dcf9b30. |
| logged-in-admin | mobile | reading.init.spread_5: created 071cc73e-5c79-4ad6-b58b-688a5f10e9d0. |
| logged-in-admin | mobile | reading.init.spread_10: created c01ce89d-3405-4c5b-ba53-2422722b518a. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created af0de376-971e-4cc9-a333-3f30db229b2d. |
| logged-in-reader | mobile | reading.init.spread_5: created 76caae04-afc0-4374-a93a-951155efd887. |
| logged-in-reader | mobile | reading.init.spread_10: created 867ca370-3c6e-4306-afdb-ab9f25cbc558. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.
