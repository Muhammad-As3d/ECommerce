from reportlab.lib import colors
from reportlab.lib.enums import TA_RIGHT, TA_LEFT, TA_CENTER
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import mm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, PageBreak, Table, TableStyle, KeepTogether
import arabic_reshaper
from bidi.algorithm import get_display
from pathlib import Path
import json

ROOT = Path(r"D:\Projects\ECommerce")
OUT = ROOT / "output" / "pdf" / "ecommerce_order_payment_backlog_ar.pdf"
OUT.parent.mkdir(parents=True, exist_ok=True)

pdfmetrics.registerFont(TTFont("Tahoma", r"C:\Windows\Fonts\tahoma.ttf"))
pdfmetrics.registerFont(TTFont("Tahoma-Bold", r"C:\Windows\Fonts\tahomabd.ttf"))

NAVY = colors.HexColor("#0F2747")
BLUE = colors.HexColor("#1F6FEB")
CYAN = colors.HexColor("#16A3B6")
LIGHT = colors.HexColor("#F3F7FC")
MID = colors.HexColor("#D9E5F3")
INK = colors.HexColor("#18212F")
MUTED = colors.HexColor("#526278")
GREEN = colors.HexColor("#16835A")
RED = colors.HexColor("#B42318")

def ar(s):
    return get_display(arabic_reshaper.reshape(str(s)))

styles = getSampleStyleSheet()
body = ParagraphStyle("ArabicBody", fontName="Tahoma", fontSize=9.2, leading=15, alignment=TA_RIGHT, textColor=INK, spaceAfter=5)
small = ParagraphStyle("ArabicSmall", parent=body, fontSize=8, leading=12, textColor=MUTED)
h1 = ParagraphStyle("ArabicH1", fontName="Tahoma-Bold", fontSize=21, leading=30, alignment=TA_RIGHT, textColor=NAVY, spaceAfter=10)
h2 = ParagraphStyle("ArabicH2", fontName="Tahoma-Bold", fontSize=15, leading=22, alignment=TA_RIGHT, textColor=BLUE, spaceBefore=8, spaceAfter=8)
h3 = ParagraphStyle("ArabicH3", fontName="Tahoma-Bold", fontSize=11.5, leading=18, alignment=TA_RIGHT, textColor=NAVY, spaceBefore=7, spaceAfter=5)
label = ParagraphStyle("Label", fontName="Tahoma-Bold", fontSize=8.5, leading=13, alignment=TA_RIGHT, textColor=NAVY)
code = ParagraphStyle("Code", fontName="Courier", fontSize=7.2, leading=10.5, alignment=TA_LEFT, textColor=INK, backColor=colors.HexColor("#F6F8FA"), borderColor=MID, borderWidth=.5, borderPadding=7, spaceAfter=7)
cover_title = ParagraphStyle("Cover", fontName="Tahoma-Bold", fontSize=26, leading=38, alignment=TA_CENTER, textColor=colors.white)
cover_sub = ParagraphStyle("CoverSub", fontName="Tahoma", fontSize=12, leading=20, alignment=TA_CENTER, textColor=colors.HexColor("#DCEBFF"))

def P(text, style=body): return Paragraph(ar(text), style)
def C(text): return Paragraph(text.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;").replace("\n", "<br/>"), code)
def bullet(text): return P("• " + text)

def info_table(rows):
    data = [[P(v, body), P(k, label)] for k,v in rows]
    t = Table(data, colWidths=[125*mm, 42*mm], hAlign="RIGHT")
    t.setStyle(TableStyle([
        ("BACKGROUND", (1,0), (1,-1), LIGHT), ("BOX", (0,0), (-1,-1), .5, MID),
        ("INNERGRID", (0,0), (-1,-1), .35, MID), ("VALIGN", (0,0), (-1,-1), "TOP"),
        ("RIGHTPADDING", (0,0), (-1,-1), 7), ("LEFTPADDING", (0,0), (-1,-1), 7),
        ("TOPPADDING", (0,0), (-1,-1), 5), ("BOTTOMPADDING", (0,0), (-1,-1), 5),
    ]))
    return t

def json_block(obj): return C(json.dumps(obj, ensure_ascii=False, indent=2))

story=[]
story += [Spacer(1, 38*mm), Table([[P("حزمة مهام دورة الطلب والدفع", cover_title)]], colWidths=[170*mm], rowHeights=[55*mm], style=[("BACKGROUND",(0,0),(-1,-1),NAVY),("VALIGN",(0,0),(-1,-1),"MIDDLE")]), Spacer(1,6*mm), P("Order History + Payment + Webhook + Status Lifecycle + Stock Restore", cover_sub), Spacer(1,15*mm), info_table([
    ("المشروع", "ECommerce API - ASP.NET Core / Clean Architecture"),
    ("نوع المستند", "Product backlog بصيغة Jira-ready"),
    ("النطاق", "Backend API + database + integration + automated tests"),
    ("الإصدار", "1.0 - 10 أغسطس 2026"),
]), Spacer(1,18*mm), P("الهدف: تحويل checkout الحالي إلى دورة طلب production-ready قابلة للتتبع والدفع الآمن والاسترجاع السليم للمخزون.", h3), PageBreak()]

story += [P("طريقة استخدام المستند", h1), P("كل قسم يمثل Epic، وتحته Stories قابلة للنقل إلى Jira. التقديرات بالنقاط تقريبية وتفترض مطور Backend واحد يعرف بنية المشروع الحالية."),
P("قرارات النطاق الأساسية", h2)]
for x in [
    "الدفع online غير متزامن: حالة الطلب لا تصبح Paid من redirect أو response صادر للعميل، بل من Webhook موثوق.",
    "Checkout ينشئ Order في AwaitingPayment ويحجز/يخصم المخزون داخل transaction واحدة.",
    "أي Webhook أو أمر استرجاع مخزون يجب أن يكون idempotent.",
    "الأسعار وأسماء المنتجات تحفظ snapshots داخل OrderItem ولا تعاد قراءتها لحساب طلب قديم.",
    "كل انتقال حالة يسجل في OrderStatusHistory مع الفاعل والسبب والتوقيت.",
]: story.append(bullet(x))
story += [P("Definition of Done العام", h2)]
for x in [
    "Endpoint موثق في OpenAPI، ومؤمن بالـpolicy المناسبة.", "FluentValidation للمدخلات مع Problem Details متسق.",
    "Unit tests لقواعد الحالة والحسابات، وintegration tests للمسار السعيد والحالات الحرجة.",
    "CancellationToken مستخدم، ولا تظهر أسرار provider أو stack traces في response.",
    "Migration قابلة للتطبيق على قاعدة نظيفة، مع indexes وunique constraints المطلوبة.",
]: story.append(bullet(x))
story += [PageBreak(), P("خريطة الحالات", h1), P("الحالات المقترحة للطلب", h2)]
state_rows = [
    [P("المعنى",label), P("الانتقالات المسموحة",label), Paragraph("Status",label)],
    [P("تم إنشاء الطلب وينتظر الدفع"), Paragraph("Paid, PaymentFailed, Cancelled", body), Paragraph("AwaitingPayment",body)],
    [P("الدفع مؤكد"), Paragraph("Processing, Refunded",body), Paragraph("Paid",body)],
    [P("جاري التجهيز"), Paragraph("Shipped, Cancelled",body), Paragraph("Processing",body)],
    [P("خرج للشحن"), Paragraph("Delivered",body), Paragraph("Shipped",body)],
    [P("تم التسليم"), Paragraph("Refunded",body), Paragraph("Delivered",body)],
    [P("فشل الدفع؛ المخزون مسترجع"), Paragraph("terminal",body), Paragraph("PaymentFailed",body)],
    [P("ملغي؛ المخزون مسترجع عند الحاجة"), Paragraph("terminal",body), Paragraph("Cancelled",body)],
    [P("تم رد المبلغ"), Paragraph("terminal",body), Paragraph("Refunded",body)],
]
t=Table(state_rows,colWidths=[62*mm,68*mm,37*mm],repeatRows=1)
t.setStyle(TableStyle([("BACKGROUND",(0,0),(-1,0),NAVY),("TEXTCOLOR",(0,0),(-1,0),colors.white),("GRID",(0,0),(-1,-1),.4,MID),("VALIGN",(0,0),(-1,-1),"TOP"),("ROWBACKGROUNDS",(0,1),(-1,-1),[colors.white,LIGHT]),("ALIGN",(-1,1),(-1,-1),"LEFT"),]))
story += [t, Spacer(1,6*mm), P("قاعدة مهمة: الانتقال يتم داخل domain method مثل Order.TransitionTo، وليس بتعديل property مباشرة. أي transition غير مسموح يرجع 409 Conflict.", h3)]

def task(title, key, points, summary, endpoint=None, auth=None, request=None, response=None, validations=None, rules=None, errors=None, acceptance=None, deps=None, priority="High"):
    story.extend([Spacer(1, 8*mm), KeepTogether([P(f"{key} - {title}", h1), info_table([("الأولوية",priority),("Story points",str(points)),("الوصف",summary)])])])
    if endpoint: story.extend([P("API Contract",h2), info_table([("Endpoint",endpoint),("Authorization",auth or "غير محدد")])])
    if request is not None: story.extend([P("Request",h2), json_block(request) if isinstance(request,(dict,list)) else C(request)])
    if response is not None: story.extend([P("Success response",h2), json_block(response) if isinstance(response,(dict,list)) else C(response)])
    for heading, items in [("Validation",validations),("Business rules",rules),("حالات الخطأ",errors),("Acceptance criteria",acceptance),("Dependencies",deps)]:
        if items:
            story.append(P(heading,h2))
            for item in items: story.append(bullet(item))

task("إضافة نموذج دورة حالة الطلب وسجلها","ORD-101",5,"تأسيس state machine قابلة للتدقيق لكل طلب.",
validations=["Status قيمة معرفة في OrderStatus enum.","Reason اختياري، وبحد أقصى 500 حرف.","ActorId مطلوب عندما يكون الانتقال يدويًا من Admin."],
rules=["إضافة AwaitingPayment وPaymentFailed وRefunded للحالات الحالية.","إنشاء OrderStatusHistory: Id, OrderId, FromStatus, ToStatus, Reason, ActorType, ActorId, CreatedOn.","تسجيل أول history عند إنشاء الطلب.","منع الانتقالات غير الموجودة في transition map.","إضافة concurrency token إلى Order لمنع تحديثين متزامنين."],
errors=["409 ORDER_INVALID_STATUS_TRANSITION.","409 ORDER_CONCURRENCY_CONFLICT."],
acceptance=["كل تغيير حالة ناجح ينتج history row واحدًا فقط.","لا يمكن الانتقال من terminal state.","اختبارات تغطي كل transition مسموح ومرفوض."])

task("عرض سجل طلبات العميل","ORD-102",5,"إرجاع قائمة paginated بملخص طلبات المستخدم الحالي.","GET /api/orders?pageNumber=1&pageSize=20&status=Paid&sort=-createdOn","Customer JWT",
response={"items":[{"id":"uuid","orderNumber":"ORD-000001","status":"Paid","createdOn":"2026-08-10T10:30:00Z","itemsCount":2,"totalAmount":1550.00,"currency":"EGP"}],"pageNumber":1,"pageSize":20,"totalCount":1,"totalPages":1},
validations=["pageNumber >= 1.","pageSize بين 1 و100.","status إن أرسل يجب أن يكون enum صالحًا.","sort محصور في createdOn وtotalAmount فقط."],
rules=["الاستعلام يعرض طلبات currentUser فقط.","NoTracking مع projection، ولا يعيد payment secrets أو بيانات مستخدم آخر."],
errors=["401 عند غياب/انتهاء JWT.","400 VALIDATION_ERROR للفلترة غير الصالحة."],
acceptance=["ترتيب افتراضي createdOn descending.","قائمة فارغة ترجع 200 وليس 404.","لا يمكن كشف طلبات مستخدم آخر بتغيير query parameters."],deps=["ORD-101"])

task("عرض تفاصيل طلب للعميل","ORD-103",3,"إرجاع snapshot كامل للطلب وعناصره والدفع وسجل الحالة.","GET /api/orders/{orderId}","Customer JWT",
response={"id":"uuid","orderNumber":"ORD-000001","status":"Paid","createdOn":"2026-08-10T10:30:00Z","shippingAddress":{"name":"Muhammad","phone":"+201...","city":"Cairo","line1":"..."},"items":[{"productId":"uuid","productName":"Phone","unitPrice":1500.00,"quantity":1,"subtotal":1500.00}],"subtotal":1500.00,"discountAmount":0,"shippingFee":50.00,"taxAmount":0,"totalAmount":1550.00,"currency":"EGP","payment":{"status":"Succeeded","provider":"Paymob","paidOn":"2026-08-10T10:32:00Z"},"statusHistory":[{"from":"AwaitingPayment","to":"Paid","createdOn":"2026-08-10T10:32:00Z"}]},
validations=["orderId يجب ألا يكون Guid.Empty."],rules=["المالك فقط يرى الطلب.","العنوان يرجع snapshot محفوظًا وقت الطلب؛ لا يتغير لو المستخدم عدل عنوانه لاحقًا."],errors=["404 ORDER_NOT_FOUND للحالة غير الموجودة أو غير المملوكة، لتجنب تسريب الوجود."],acceptance=["response لا يعتمد على بيانات المنتج الحالية.","كل totals تطابق snapshots المخزنة."],deps=["ORD-101"])

task("قائمة الطلبات للأدمن","ORD-104",5,"بحث وفلترة تشغيلية لكل الطلبات.","GET /api/admin/orders?pageNumber=1&pageSize=20&status=Paid&from=...&to=...&search=ORD-1","Admin role",
response={"items":[{"id":"uuid","orderNumber":"ORD-000001","customer":{"id":"user-id","email":"u@example.com"},"status":"Paid","paymentStatus":"Succeeded","totalAmount":1550.00,"createdOn":"..."}],"pageNumber":1,"pageSize":20,"totalCount":42,"totalPages":3},
validations=["pageSize <= 100.","from <= to.","الفترة القصوى 366 يومًا.","status وpaymentStatus enums صالحة."],rules=["البحث برقم الطلب أو بريد العميل.","Indexes على OrderNumber, Status, CreatedOn, UserId."],errors=["403 لغير Admin.","400 للفترة أو enum غير الصالح."],acceptance=["Pagination ثابتة مع tie-breaker على Id.","الفلترة مجمعة تعمل معًا دون client evaluation."],deps=["ORD-101"])

task("تغيير حالة الطلب بواسطة الأدمن","ORD-105",5,"تنفيذ transition يدوي مسجل وقابل للتزامن.","PATCH /api/admin/orders/{orderId}/status","Admin role",
request={"status":"Processing","reason":"Payment verified and packing started","expectedVersion":"base64-row-version"},response={"id":"uuid","status":"Processing","version":"new-base64-row-version","updatedOn":"..."},
validations=["status مطلوب وصالح.","reason <= 500.","expectedVersion مطلوب."],rules=["استدعاء domain transition فقط.","Paid إلى Processing مسموح؛ AwaitingPayment إلى Processing مرفوض.","تسجيل Admin user id في history.","إرسال OrderStatusChanged event بعد commit."],errors=["404 ORDER_NOT_FOUND.","409 ORDER_INVALID_STATUS_TRANSITION.","409 ORDER_CONCURRENCY_CONFLICT."],acceptance=["نفس request مع version قديم يفشل 409.","لا يوجد history عند فشل transaction."],deps=["ORD-101"])

task("إنشاء Payment Session أثناء Checkout","PAY-201",8,"ربط checkout بمزود دفع مع abstraction قابلة لتبديل Stripe أو Paymob.","POST /api/orders/checkout","Customer JWT + Idempotency-Key header",
request={"shippingAddressId":"uuid","paymentMethod":"Card","currency":"EGP","returnUrl":"https://store.example.com/payment/result"},response={"orderId":"uuid","orderNumber":"ORD-000001","status":"AwaitingPayment","payment":{"id":"uuid","status":"Pending","provider":"Paymob","checkoutUrl":"https://provider/...","expiresOn":"2026-08-10T11:00:00Z"},"totalAmount":1550.00,"currency":"EGP"},
validations=["Idempotency-Key مطلوب، 16-128 حرفًا.","shippingAddressId صالح ومملوك للمستخدم.","paymentMethod من القيم المدعومة.","currency ضمن العملات المسموحة.","returnUrl HTTPS ومن allowlist configured domains.","السلة غير فارغة وكل quantity > 0."],
rules=["إعادة قراءة السعر والمخزون من DB داخل transaction؛ لا تثق في subtotal القادم من cart projection.","إنشاء Order + items snapshots + Payment Pending.","إرسال amount بأصغر وحدة نقدية للمزود.","تخزين ProviderPaymentId مع unique index.","لو فشل provider قبل commit لا يترك order ناقصًا؛ لو بعد commit يسجل PaymentFailed ويسترجع المخزون.","نفس Idempotency-Key لنفس المستخدم يعيد نفس response."],
errors=["400 CART_EMPTY / ADDRESS_NOT_FOUND / UNSUPPORTED_CURRENCY.","409 INSUFFICIENT_STOCK.","409 IDEMPOTENCY_KEY_REUSED_WITH_DIFFERENT_PAYLOAD.","502 PAYMENT_PROVIDER_UNAVAILABLE."],
acceptance=["لا ينشأ أكثر من order لنفس المفتاح.","المبلغ لدى provider يساوي TotalAmount المخزن.","لا يرجع secret داخلي أو webhook secret للعميل."],deps=["ORD-101","STK-301","INF-501"])

task("Webhook للدفع الناجح أو الفاشل","PAY-202",8,"استقبال أحداث provider بشكل آمن وidempotent.","POST /api/payments/webhooks/{provider}","Public endpoint؛ provider signature mandatory",
request="Raw provider payload + signature headers. Do not model-bind before signature verification.",response="HTTP 200 after event is durably accepted. Duplicate known event also returns 200.",
validations=["provider ضمن providers المسجلة.","التحقق من signature على raw body وبـconstant-time comparison حيث يلزم.","التحقق من timestamp/tolerance لمنع replay.","ProviderEventId وProviderPaymentId غير فارغين.","amount وcurrency يطابقان Payment المخزن."],
rules=["حفظ WebhookEvent بunique(provider, eventId) قبل المعالجة.","Succeeded: Payment -> Succeeded ثم Order -> Paid.","Failed/expired: Payment -> Failed ثم Order -> PaymentFailed ثم stock restore مرة واحدة.","الأحداث خارج الترتيب لا ترجع الحالة للخلف.","لا تسجل raw payload إذا احتوى بيانات حساسة؛ خزّن نسخة منقحة أو hash."],
errors=["400 WEBHOOK_INVALID_PAYLOAD.","401 WEBHOOK_INVALID_SIGNATURE.","200 للحدث المكرر المعروف.","500 فقط للأخطاء المؤقتة كي يعيد provider المحاولة."],
acceptance=["تكرار نفس event عشر مرات يحدث تغييرًا واحدًا.","signature غير صحيحة لا تغير DB.","نجاح الدفع يضيف history من AwaitingPayment إلى Paid.","فشل الدفع يسترجع المخزون مرة واحدة."],deps=["PAY-201","STK-302"])

task("استعلام حالة الدفع","PAY-203",3,"تمكين الواجهة من polling آمن بعد الرجوع من provider.","GET /api/orders/{orderId}/payment","Order owner JWT",
response={"orderId":"uuid","orderStatus":"Paid","paymentStatus":"Succeeded","paidOn":"...","lastUpdatedOn":"..."},
validations=["orderId Guid صالح."],rules=["الـendpoint يقرأ الحالة المحلية فقط ولا يستعلم provider في كل request.","لا يعيد provider client secret أو raw response."],errors=["404 إذا الطلب غير موجود أو غير مملوك."],acceptance=["الحالة تتحدث بعد معالجة webhook.","يعمل حتى لو أغلق العميل صفحة الدفع."],deps=["PAY-202"])

task("إلغاء طلب غير مشحون","ORD-106",5,"إلغاء ذاتي للعميل مع stock restore/refund حسب الحالة.","POST /api/orders/{orderId}/cancel","Order owner JWT + Idempotency-Key",
request={"reason":"Changed my mind"},response={"orderId":"uuid","status":"Cancelled","refundStatus":"NotRequired","cancelledOn":"..."},
validations=["reason مطلوب بين 3 و500 حرف.","Idempotency-Key صالح."],rules=["AwaitingPayment: cancel payment intent إن أمكن ثم restore stock.","Paid/Processing: يبدأ refund ثم يصبح Cancelled فقط حسب سياسة واضحة؛ Shipped/Delivered لا يلغى من هذا endpoint.","تسجيل السبب والفاعل Customer.","التكرار يعيد نفس النتيجة."],errors=["404 ORDER_NOT_FOUND.","409 ORDER_CANNOT_BE_CANCELLED.","409 REFUND_REQUIRED أو 502 عند فشل provider وفق الاستراتيجية المختارة."],acceptance=["إلغاء AwaitingPayment يعيد المخزون مرة واحدة.","لا يمكن إلغاء Shipped.","الحالة والتاريخ والمخزون تتسق transactionally."],deps=["ORD-101","STK-302","PAY-204"])

task("Refund كامل بواسطة الأدمن","PAY-204",8,"رد المبلغ للطلبات المدفوعة مع تتبع provider.","POST /api/admin/orders/{orderId}/refund","Admin role + Idempotency-Key",
request={"amount":1550.00,"reason":"Order cancelled before shipping"},response={"refundId":"uuid","orderId":"uuid","status":"Pending","amount":1550.00,"currency":"EGP","providerReference":"..."},
validations=["amount > 0 ولا يتجاوز paidAmount - refundedAmount.","reason بين 3 و500.","الطلب مدفوع وقابل للـrefund.","Idempotency-Key مطلوب."],rules=["إنشاء Refund entity وحفظ provider reference.","لا تعتبر الطلب Refunded حتى webhook confirmation إن كان provider asynchronous.","دعم partial refund في schema حتى لو الـMVP يسمح الكامل فقط.","الـrefund لا يعيد المخزون تلقائيًا بعد Delivered؛ قرار stock منفصل عند return inspection."],errors=["409 PAYMENT_NOT_REFUNDABLE / REFUND_AMOUNT_EXCEEDED.","502 PAYMENT_PROVIDER_UNAVAILABLE."],acceptance=["التكرار لا ينشئ refund ثانيًا.","عند التأكيد يصبح Payment Refunded والطلب Refunded إذا كان كاملًا."],deps=["PAY-202"])

task("تسجيل حركة المخزون وحجزه","STK-301",8,"استبدال الخصم المجرد بسجل inventory قابل للتدقيق.",
validations=["QuantityDelta لا يساوي صفرًا.","ProductId موجود.","ReferenceId مطلوب للحركات المرتبطة بطلب."],rules=["إنشاء InventoryMovement: ProductId, Type, QuantityDelta, BalanceAfter, ReferenceType, ReferenceId, IdempotencyKey, CreatedOn.","Unique على Type + ReferenceId + ProductId لمنع التكرار.","Checkout يخصم atomically بشرط Stock >= quantity ويسجل Reservation/OrderAllocation.","كل حركة تتم داخل transaction نفسها مع تحديث Product.Stock.","اختياري لاحقًا: ReservedStock وAvailableStock بدل الخصم المباشر."],errors=["409 INSUFFICIENT_STOCK.","409 INVENTORY_CONCURRENCY_CONFLICT."],acceptance=["مجموع الحركات يفسر stock الحالي.","طلبان متزامنان على آخر قطعة: ينجح واحد فقط.","لا توجد قيمة stock سالبة."],deps=["Migration + specialized repository update"])

task("استرجاع المخزون Idempotently","STK-302",5,"إرجاع كميات الطلب عند الفشل أو الإلغاء دون double restore.",
validations=["OrderId موجود.","سبب الاسترجاع من PaymentFailed أو Cancelled أو Expired.","كل OrderItem quantity موجب."],rules=["RestoreStock(orderId, reason) ينشئ movement موجب لكل item.","unique constraint يمنع استرجاع نفس order/reason/product مرتين.","التنفيذ داخل transaction مع تغيير الحالة عند الإمكان.","لا يسترجع stock لطلب Paid إلا ضمن cancel/refund policy.","يسجل restoredOn أو StockRestored flag كقراءة مساعدة، والـmovement هو مصدر التدقيق."],errors=["409 STOCK_ALREADY_RESTORED يمكن معاملته نجاحًا idempotent داخليًا.","409 ORDER_NOT_ELIGIBLE_FOR_STOCK_RESTORE."],acceptance=["Webhook فشل متكرر لا يزيد stock أكثر من الأصل.","فشل منتصف العملية يؤدي rollback لكل العناصر.","اختبار طلب يحتوي أكثر من Product."],deps=["STK-301"])

task("إنهاء الطلبات منتهية الدفع","JOB-401",5,"Background job يعالج AwaitingPayment التي تجاوزت مهلة الدفع.",
validations=["PaymentExpirationMinutes قيمة موجبة ومحدودة.","BatchSize بين 1 و500."],rules=["يجلب batches مع locking مناسب.","يتحقق من الحالة الحالية قبل التغيير.","يحاول cancel عند provider ثم يضع Payment Expired وOrder PaymentFailed ويسترجع stock.","العملية idempotent وقابلة لإعادة المحاولة.","تسجيل metrics لعدد الطلبات المنتهية والفشل."],errors=["provider timeout يؤدي retry مع backoff ولا يكرر stock restore."],acceptance=["طلب مدفوع قبل تشغيل job لا يتم إلغاؤه.","تشغيل job مرتين يعطي نفس النتيجة.","يوجد integration test بتوقيت قابل للتحكم."],deps=["PAY-202","STK-302","Background job host"])

task("البنية التحتية والـschema","INF-501",5,"إضافة migrations وقيود البيانات المطلوبة للدورة كاملة.",
rules=["Payment: Provider, ProviderPaymentId, Status, Amount, Currency, FailureCode, PaidOn, RowVersion.","WebhookEvent: Provider, EventId, Type, PayloadHash, ReceivedOn, ProcessedOn, ProcessingError.","IdempotencyRecord: UserId, Key, RequestHash, StatusCode, ResponseBody, ExpiresOn.","OrderStatusHistory وInventoryMovement وRefund حسب القصص السابقة.","Unique indexes: OrderNumber؛ Provider+ProviderPaymentId؛ Provider+EventId؛ UserId+IdempotencyKey.","Decimal precision موحد، وCurrency بطول 3، وtimestamps UTC."],acceptance=["migration تطبق على empty DB وعلى نسخة schema الحالية.","كل FK delete behavior مقصود ومختبر.","rollback script أو documented rollback strategy موجودة."],deps=["قرارات provider وسياسة الاحتفاظ بالبيانات"])

task("الاختبارات والأمان والمراقبة","QA-601",8,"تغطية production-critical paths قبل الإطلاق.",
rules=["Unit: state machine، totals، refundable amount، restore eligibility.","Integration: checkout concurrency، idempotency، ownership، admin authorization.","Webhook contract tests باستخدام payloads موقعة من provider sandbox.","اختبارات replay/out-of-order/duplicate webhook.","Structured logs تحتوي OrderId/PaymentId/EventId دون tokens أو card data.","Metrics: payment success/failure، webhook lag، stock restore، invalid signatures.","Health check للمزود لا يرسل عملية مالية حقيقية."],acceptance=["كل critical scenarios تمر في CI.","لا يوجد real provider secret في repository أو test snapshots.","Runbook يشرح إعادة معالجة webhook فاشل ومطابقة payment يدويًا."],deps=["كل القصص السابقة"])

story += [Spacer(1, 10*mm), P("ترتيب التنفيذ المقترح", h1)]
for x in ["Sprint 1: ORD-101, INF-501, STK-301, ORD-102, ORD-103.","Sprint 2: PAY-201, PAY-202, STK-302, PAY-203.","Sprint 3: ORD-104, ORD-105, ORD-106, PAY-204, JOB-401.","مستمر عبر السبرنتات: QA-601 واختبارات كل story قبل إغلاقها."]:
    story.append(bullet(x))
story += [P("أسئلة Product يجب حسمها قبل Payment implementation",h2)]
for x in ["مزود الدفع: Paymob أم Stripe؟", "هل الدفع Card فقط أم Cash on Delivery أيضًا؟", "مدة صلاحية payment session وحجز المخزون؟", "هل يسمح بإلغاء Processing؟ ومن يتحمل رسوم الـrefund؟", "هل partial refunds مطلوبة في أول release؟", "العملة الواحدة EGP أم multi-currency؟", "هل المخزون يعود فور الإلغاء أم بعد فحص المرتجع للطلبات المسلمة؟"]:
    story.append(bullet(x))
story += [P("ملاحظة على الكود الحالي",h2), P("الـcheckout الحالي يخصم المخزون atomically داخل transaction، وهي بداية جيدة. المطلوب هو تغليفه بسجل حركة، idempotency، lifecycle للدفع، واسترجاع مضمون للمخزون. كما ينبغي إنشاء snapshot فعلي للعنوان داخل الطلب بدل الاعتماد طويلًا على Address entity القابلة للتعديل أو الحذف.")]

def header_footer(canvas, doc):
    canvas.saveState()
    if doc.page > 1:
        canvas.setStrokeColor(MID); canvas.line(20*mm, 282*mm, 190*mm, 282*mm)
        canvas.setFont("Tahoma",7.5); canvas.setFillColor(MUTED)
        canvas.drawString(20*mm, 12*mm, f"ECommerce API - Jira Backlog")
        canvas.drawRightString(190*mm, 12*mm, f"{doc.page}")
    canvas.restoreState()

doc=SimpleDocTemplate(str(OUT),pagesize=A4,rightMargin=20*mm,leftMargin=20*mm,topMargin=18*mm,bottomMargin=18*mm,title="ECommerce Order & Payment Backlog",author="Codex")
doc.build(story,onFirstPage=header_footer,onLaterPages=header_footer)
print(OUT)
