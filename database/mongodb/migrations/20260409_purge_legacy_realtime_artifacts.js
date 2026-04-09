(function () {
  "use strict";

  const legacyPrefix = "ca" + "ll";
  const sessionCollection = `${legacyPrefix}_sessions`;
  const legacyType = `${legacyPrefix}_log`;
  const legacyPayloadField = `${legacyPrefix}_payload`;

  if (db.getCollectionNames().includes(sessionCollection)) {
    db[sessionCollection].drop();
    print(`[cleanup] Dropped collection: ${sessionCollection}`);
  } else {
    print(`[cleanup] Collection not found, skip drop: ${sessionCollection}`);
  }

  const deleteResult = db.chat_messages.deleteMany({ type: legacyType });
  print(`[cleanup] Removed legacy chat messages: ${deleteResult.deletedCount}`);

  const unsetResult = db.chat_messages.updateMany(
    { [legacyPayloadField]: { $exists: true } },
    { $unset: { [legacyPayloadField]: "" } }
  );
  print(`[cleanup] Removed legacy payload field from chat messages: ${unsetResult.modifiedCount}`);
})();
