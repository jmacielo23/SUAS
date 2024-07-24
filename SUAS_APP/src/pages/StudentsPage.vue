<template>
  <q-page>
  <q-dialog v-model="showEditDialog">
    <q-card>
      <q-card-section>
        <q-input label="ID" v-model="selectedStudent.id" readonly />
        <q-input label="First Name" v-model="selectedStudent.firstName" :rules="[val => !!val || 'This field is required']"/>
        <q-input label="Last Name" v-model="selectedStudent.lastName" :rules="[val => !!val || 'This field is required']"/>
        <q-input label="Date of Birth" v-model="selectedStudent.dateOfBirth" :rules="[val => !!val || 'This field is required']"  
          @click="openDOBCalendarEdit()" readonly />
        <q-input label="Email" v-model="selectedStudent.email" />
        <q-input label="Phone Number" v-model="selectedStudent.phoneNumber" />
      </q-card-section>
      <q-card-actions align="right">
        <q-btn label="Cancel" color="negative" @click="cancelEdit" />
        <q-btn label="Save Changes" color="positive" @click="saveChanges" />
      </q-card-actions>
    </q-card>
  </q-dialog>
  <q-dialog v-model ="showDOBPicker">
    <q-card>
      <q-card-section>
        <q-date
          v-model="newStudent.dateOfBirth" minimal
        />
      </q-card-section>
      <q-card-actions align="right">
        <q-btn color="negative" label="Close" @click="showDOBPicker = false" />
      </q-card-actions>
    </q-card>
  </q-dialog>
  <q-dialog v-model ="showDOBPickerEdit">
      <q-card>
      <q-card-section>
        <q-date
          v-model="newStudent.dateOfBirth" minimal
        />
      </q-card-section>
      <q-card-actions align="right">
        <q-btn color="negative" label="Close" @click="showDOBPickerEdit = false" />
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
      <h5> Create Student </h5>
      <hr>
      <q-form @submit.prevent="createStudent">      
        <q-input label="ID" v-model="newStudent.id" :rules="[val => !!val || 'This field is required']"/>
        <q-input label="First Name" v-model="newStudent.firstName" :rules="[val => !!val || 'This field is required']"/>
        <q-input label="Last Name" v-model="newStudent.lastName" :rules="[val => !!val || 'This field is required']"/>     
        <div class="q-field">
            <q-input v-model="newStudent.dateOfBirth" label="Date of Birth"
             @click="openDOBCalendar()" 
             :rules="[val => !!val || 'This field is required']" readonly />
        </div>      
        <q-input label="Email" v-model="newStudent.email" />
        <q-input label="Phone Number" v-model="newStudent.phoneNumber" />
        <hr>
        <q-btn label="Create Student" color="positive" type="submit" />
        <hr>
      </q-form> 
    </div> 
    <q-table :rows="students" :columns="columns">
      <template v-slot:body-cell-delete="props">
        <q-td :props="props">
        <q-btn color="primary" label="Edit" @click="editStudent(props.row)" />
        <q-btn color="negative" label="Delete" @click="deleteStudent(props.row.id)" />
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
      students: [],
      newStudent: {
        id: '',
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        email: '',
        phoneNumber: ''        
      },
      showEditDialog: false,
      showDOBPicker: false,
      showDOBPickerEdit: false,
      errorDialog: false,
      errorMessage: '',
      confirmDialog: false,
      confirmMessage: '',
      selectedStudent: {
        id: '',
        firstName: '',
        lastName: '',
        dateOfBirth: '',
        email: '',
        phoneNumber: '' 
      },
      columns: [
                {
          name: 'id',
          align: 'left',
          label: 'ID',
          field: 'id',
        },
        {
          name: 'firstName',
          required: true,
          label: 'Name',
          align: 'left',
          field: row => row.firstName,
          format: val => `${val}`,
        },
        {
          name: 'lastName',
          required: true,
          label: 'Last Name',
          align: 'left',
          field: row => row.lastName,
          format: val => `${val}`,
        },
        {
          name: 'dateOfBirth',
          required: true,
          label: 'Birth Date',
          align: 'left',
          field: row => row.dateOfBirth,
          format: val => `${val}`,
        },
        {
          name: 'email',
          required: true,
          label: 'Email',
          align: 'left',
          field: row => row.email,
          format: val => `${val}`,
        },
        {
          name: 'phoneNumber',
          required: true,
          label: 'Phone Number',
          align: 'left',
          field: row => row.phoneNumber,
          format: val => `${val}`,
        },
        {
          name: 'delete',
          label: 'Actions',
          align: 'center',
        }
        // Add more columns as needed
      ],
    };
  },
  mounted() {
    this.fetchStudents();
  },
  methods: {
    async fetchStudents() {
      try {
        const response = await axios.get('https://localhost:44319/api/Students');
        this.students = response.data;
      } catch (error) {
        console.error('Error fetching students:', error);
      }
    },
    async createStudent() {
      try {
        this.formatDate();
        await axios.post('https://localhost:44319/api/Students', this.newStudent);
        this.fetchStudents(); // Refresh the student list after creating a new student
        this.newStudent = { firstName: '', id: '', lastName: '', dateOfBirth: '', email:'',phoneNumber:'' }; // Reset the form fields
        this.confirmMessage='Record Saved!';
        this.confirmDialog=true;
      } catch (error) {
          if(error.response.data.errors==null){
            this.errorMessage = error.response.data;
          }
          else {
            this.errorMessage = error.response.data.errors;
          }
        this.errorDialog = true;
        console.error('Error creating student:', error);
      }
    },
    editStudent(student) {
      this.selectedStudent = Object.assign({}, student);
      this.showEditDialog = true;
    },
    cancelEdit() {
    this.selectedStudent = {
      id: '',
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      email: '',
      phoneNumber: ''
    };
    this.showEditDialog = false;
  },
  async saveChanges() {
    try {
      await axios.put(`https://localhost:44319/api/Students/${this.selectedStudent.id}`, this.selectedStudent);
      this.fetchStudents(); // Refresh the student list after editing a student
      this.showEditDialog = false;
      this.confirmMessage='Record Updated!';
      this.confirmDialog=true;
    } catch (error) {
      if(error.response.data.errors==null){
          this.errorMessage = error.response.data;
        }
        else {
          this.errorMessage = error.response.data.errors;
        }
      this.errorDialog = true;
      console.error('Error editing student:', error);
    }
  },
    async deleteStudent(id) {
      try {
        await axios.delete(`https://localhost:44319/api/Students/${id}`);
        this.students = this.students.filter(student => student.id !== id);
      } catch (error) {
        console.error('Error deleting student:', error);
      }
    },
    openDOBCalendar(){
      this.showDOBPicker=true;
    },
    openDOBCalendarEdit(){
      this.showDOBPickerEdit=true;
    },
    closeDOBCalendar(){
      this.showDOBPicker=false;
      this.showDOBPickerEdit=false;
    },
    formatDatePickerValue(date) {
      // Transform the date string from YYYY/MM/DD to YYYY-MM-DD format
      return date.replace(/\//g, '-');
    },
    formatDate() {
      // Format the date when input is changed
      console.log('TST');
      this.newStudent.dateOfBirth = this.formatDatePickerValue(this.newStudent.dateOfBirth);
    },
    formatDateEdit() {
      // Format the date when input is changed
      console.log('TST');
      this.selectedStudent.dateOfBirth = this.formatDatePickerValue(this.selectedStudent.dateOfBirth);
    }
  }
};
</script>

<style scoped>
/* Add your component-specific styles here */
</style>
