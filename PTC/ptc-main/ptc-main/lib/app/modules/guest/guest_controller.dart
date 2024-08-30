import '../../constants/constants.dart';
import '../../services/services.dart';
import 'guest.dart';

class GuestController extends BaseController {
  final guestP = GuestProvider();
  late Guest guest;

  @override
  void generateForm() {
    form.assignAll([
      {'title': "Guest ID", 'value': guest.nationalId},
      {'title': "Guest Name", 'value': guest.guestName},
      {'title': "Guest Company", 'value': guest.guestCompany},
      {'title': "Guest Department/Section", 'value': guest.guestDeptSect},
      {'title': "Start Date", 'value': guest.startDate},
      {'title': "End Date", 'value': guest.endDate},
      {'title': "Employee Name", 'value': guest.metName},
      {'title': "Employee Department", 'value': guest.metDept},
      {'title': "Employee Section", 'value': guest.metSect},
      // {'title': "Purpose of Visit", 'value': guest.requirement},
      {
        'title': "Total Guests",
        'value': guest.total,
        'controller': TextEditingController(text: "${guest.total}"),
        'formType': TextInputType.number,
        'length': 3,
      },
    ]);
  }

  @override
  void generateModel(String json) {
    guest = Guest.fromJson(jsonDecode(json));
  }

  @override
  getData(String id, String mode) => guestP.getData(id);

  @override
  patchData(String mode) async {
    switch (mode) {
      case "In":
        return await guestP.patchData(guest.id, "${guest.total}", mode);
      case "Out":
        return await guestP.patchData(guest.id, "", mode);
      default:
        return await guestP.getData(guest.id);
    }
  }
}
