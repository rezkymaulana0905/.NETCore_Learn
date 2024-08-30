
import '../../constants/constants.dart';
import '../../services/services.dart';
import 'contractor.dart';

class ContractorController extends BaseController {
  final contractorP = ContractorProvider();
  late Contractor contractor;

  @override
  void generateForm() {
    form.assignAll([
      {'title': "Registration Number", 'value': contractor.regNum},
      {'title': "Title", 'value': contractor.title},
      {'title': "Company Name", 'value': contractor.compName},
      {'title': "Location", 'value': contractor.location},
      {'title': "Name", 'value': contractor.name},
      {'title': "National ID", 'value': contractor.nationalId},
      {'title': "Start Date", 'value': contractor.start},
      {'title': "End Date", 'value': contractor.end},
      {'title': "In Time", 'value': contractor.inTime},
      {'title': "Out Time", 'value': contractor.outTime},
    ]);
  }

  @override
  void generateModel(String json) {
    contractor = Contractor.fromJson(jsonDecode(json));
  }

  @override
  getData(String id, String mode) => contractorP.getData(id);

  @override
  patchData(String mode) => contractorP.patchData(contractor.id, mode);
}
