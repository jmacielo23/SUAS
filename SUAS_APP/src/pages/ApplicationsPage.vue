<template>
  <q-page>
    <q-dialog v-model="showEditDialog">
      <q-card>
        <q-card-section>
          <q-input label="ID" v-model="selectedApplication.id" readonly />
          <q-input label="Student ID" v-model="selectedApplication.studentID" :rules="[val => !!val || 'This field is required']" />
          <q-input label="Course ID" v-model="selectedApplication.courseID" :rules="[val => !!val || 'This field is required']" />
          <q-input label="Application Date" v-model="selectedApplication.applicationDate" readonly />
        </q-card-section>
        <q-card-actions align="right">
          <q-btn label="Cancel" color="negative" @click="cancelEdit" />
          <q-btn label="Save Changes" color="positive" @click="saveChanges" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="errorDialog">
      <q-card>
        <q-card-section class="text-negative">{{ errorMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="negative" label="Close" @click="errorDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <q-dialog v-model="confirmDialog">
      <q-card>
        <q-card-section class="text-negative">{{ confirmMessage }}</q-card-section>
        <q-card-actions align="right">
          <q-btn color="positive" label="Ok" @click="confirmDialog = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <div class="q-pa-md" style="max-width: 800px">
      <h5> Create Application </h5>
      <hr>
      <q-form @submit.prevent="createApplication">
        <q-input label="ID" v-model="newApplication.id" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Student ID" v-model="newApplication.studentID" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Course ID" v-model="newApplication.courseID" :rules="[val => !!val || 'This field is required']" />
        <q-input label="Application Date" v-model="newApplication.applicationDate" readonly />
        <hr>
        <q-btn label="Create Application" color="positive" type="submit" />
        <hr>
      </q-form>
    </div>
    <q-table :rows="processedApplications" :columns="columns">
      <template v-slot:body-cell-delete="props">
        <q-td :props="props">
          <q-btn color="primary" label="Edit" @click="editApplication(props.row)" />
          <q-btn color="negative" label="Delete" @click="deleteApplication(props.row.id)" />
        </q-td>
      </template>
    </q-table>
  </q-page>
</template>
<script>
  import axios from 'axios';
  export default {
    data() {
      return {
        applications: [],
        showEditDialog: false,
        errorDialog: false,
        errorMessage: '',
        confirmDialog: false,
        confirmMessage: '',
        processedApplications: [],
        newApplication: {
          id: '',
          studentID: '',
          courseID: '',
          applicationDate: this.getCurrentFormattedDate()
        },
        selectedApplication: {
          id: '',
          studentID: '',
          courseID: '',
          applicationDate: ''
        },
        columns: [
          {
            name: 'id',
            align: 'left',
            label: 'ID',
            field: 'id',
          },
          {
            name: 'studentName',
            required: true,
            label: 'Student Name',
            align: 'left',
            field: row => row.studentName,
            format: val => `${val}`,
          },
          {
            name: 'courseCode',
            required: true,
            label: 'Course Code',
            align: 'left',
            field: row => row.courseCode,
            format: val => `${val}`,
          },
          {
            name: 'applicationDate',
            required: true,
            label: 'Application Date',
            align: 'left',
            field: row => row.applicationDate,
            format: val => `${val}`,
          }, ,
          {
            name: 'delete',
            label: 'Actions',
            align: 'center',
          }
        ],
      };
    },
    mounted() {
      this.fetchApplications();
    },
    methods: {
      getCurrentFormattedDate() {
        // Get the current date
        const date = new Date();

        // Format the date as "YYYY-DD-MM"
        const year = date.getFullYear();
        const day = ('0' + date.getDate()).slice(-2); // Ensure 2-digit format
        const month = ('0' + (date.getMonth() + 1)).slice(-2); // Month starts from 0

        return `${year}-${month}-${day}`;
      },
      async fetchApplications() {
        try {
          const response = await axios.get('https://localhost:44319/api/Applications');
          this.applications = response.data;
          this.processedApplications.length = 0;
          this.applications.forEach(async (application) => {
            const processedData = {
              id: application.id,
              studentID: application.studentID,
              courseID: application.courseID,
              studentName: await this.getStudentName(application.studentID),
              courseCode: await this.getCourseCode(application.courseID),
              applicationDate: application.applicationDate
            };
            this.processedApplications.push(processedData);
          }
          );
          console.log(this.processedApplications);
        }
        catch (error) {
          console.error('Error fetching courses:', error);
        }
      },
      async createApplication() {
        try {
          await axios.post('https://localhost:44319/api/Applications', this.newApplication);
          this.fetchApplications();
          this.newApplication = { id: '', studentID: '', courseID: '', applicationDate: this.getCurrentFormattedDate() };
          this.confirmMessage = 'Record Saved!';
          this.confirmDialog = true;
        }
        catch (error) {
          console.error('Error creating course', error);
          if (error.response.data.errors == null) {
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
          this.errorDialog = true;
        }
      },
      async editApplication(application) {
        this.selectedApplication = Object.assign({}, application);
        this.showEditDialog = true;
      },
      cancelEdit() {
        this.selectedApplication = {
          id: '',
          studentID: '',
          courseID: '',
          applicationDate: ''
        };
        this.showEditDialog = false;
      },
      async saveChanges() {
        try {
          await axios.put(`https://localhost:44319/api/Applications/${this.selectedApplication.id}`, this.selectedApplication);
          this.fetchApplications();
          this.showEditDialog = false;
          this.confirmMessage = 'Record Updated!';
          this.confirmDialog = true;
        }
        catch (error) {
          console.error('Error in Updating the record', error);
          if (error.response.data.errors == null) {
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
          this.errorDialog = true;
        }
      },
      async deleteApplication(id) {
        try {
          await axios.delete(`https://localhost:44319/api/Applications/${id}`);
          this.processedApplications = this.processedApplications.filter(processedApplications => processedApplications.id != id);
        }
        catch (error) {
          console.error('Error in Deleting the record', error);
        }
      },
      async getStudentName(studentID) {
        try {
          const response = await axios.get(`https://localhost:44319/api/Students/${studentID}`);
          return response.data.firstName;
        }
        catch (error) {
          console.error('Unable to get the student data!', error);
        }
      },
      async getCourseCode(courseID) {
        try {
          const response = await axios.get(`https://localhost:44319/api/Courses/${courseID}`);
          return response.data.code;
        }
        catch (error) {
          console.error('Unable to get the student data!', error);
        }
      }
    }
  };
</script>
<style scoped>
</style>
